using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;

using AutoMapper;

using Domain;

using FluentValidation.Results;

using MediatR;

using Newtonsoft.Json;

namespace Application.Clients.Command.UpdateClient
{
    public class UpdateClientCommand : IRequest<ClientDto>
    {
        [JsonIgnore]
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

     public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, ClientDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateClientCommandHandler(IApplicationDbContext context,
                                           IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
           var client = await ValidateClient(request.Id, cancellationToken);

            client.FirstName = request.FirstName ?? client.FirstName;
            client.LastName = request.LastName ?? client.LastName;
            client.Email = request.Email ?? client.Email;
            client.PhoneNumber = request.PhoneNumber ?? client.PhoneNumber;

            await _context.SaveChangesAsync(cancellationToken);

            EmailSender.sendEmail(client.Email,"Carepatron Newsletter","Hi there - welcome to my Carepatron portal.");

            return _mapper.Map<ClientDto>(client);
        }

        private async Task<Client> ValidateClient(long id, CancellationToken cancellationToken)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(client => client.Id == id, cancellationToken);

            if (client == null) throw new ValidationException(new ValidationFailure(nameof(Client), $"Client with Id:{client.Id} does not exist"));

            return client;
        }
    }
}