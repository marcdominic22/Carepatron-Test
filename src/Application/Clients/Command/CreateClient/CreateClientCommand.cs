using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Interfaces;

using AutoMapper;

using Domain;

using FluentValidation.Results;

using MediatR;
using Application.Common.Helpers;

namespace Application.Clients.Command.CreateClient
{
    public class CreateClientCommand : IRequest<ClientDto>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

     public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, ClientDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreateClientCommandHandler(IApplicationDbContext context,
                                           IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ClientDto> Handle(CreateClientCommand request, CancellationToken cancellationToken)
        {
            await ValidateClient(request.FirstName,request.LastName, cancellationToken);

            var client = new Client()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber
            };

            _context.Clients.Add(client);

            await _context.SaveChangesAsync(cancellationToken);

            EmailSender.sendEmail(client.Email,"Carepatron Newsletter","Hi there - welcome to my Carepatron portal.");
            return _mapper.Map<ClientDto>(client);
        }

        private async Task<Client> ValidateClient(string firstName, string lastName, CancellationToken cancellationToken)
        {
            var client = await _context.Clients.FirstOrDefaultAsync(client => client.FirstName == firstName &&
                                                                            client.LastName == lastName, cancellationToken);

            if (client != null) throw new ValidationException(new ValidationFailure(nameof(Client), $"Client with Id:{client.Id} and Name:{firstName} {lastName} already exist"));

            return client;
        }
    }
}