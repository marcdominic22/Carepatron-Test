using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Mappings;

using AutoMapper;

using Domain;

using FluentValidation.Results;

using MediatR;

namespace Application.Clients.Query
{
    public class GetClientQuery : IRequest<List<ClientDto>>
    {
        /// <summary>
        /// Filter Clients by keyword
        /// </summary>
        public string String { get; set; }
    }

    public class GetClientQueryHandler : IRequestHandler<GetClientQuery, List<ClientDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetClientQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ClientDto>> Handle(GetClientQuery request, CancellationToken cancellationToken)
        {
            request.String = StringExtension.ToLowerFirst(request.String);

            var clients = await _context.Clients.Where(client => client.FirstName.ToLower().Contains(request.String) || 
                                                                 client.LastName.ToLower().Contains(request.String) ||
                                                                 client.Email.ToLower().Contains(request.String) ||
                                                                 client.PhoneNumber.ToLower().Contains(request.String) ||
                                                                 client.FirstName.ToLower().Contains(request.String))
                                                .ProjectToListAsync<ClientDto>(_mapper.ConfigurationProvider);

            if (clients.Count < 0) throw new ValidationException(new ValidationFailure(nameof(Client), $"Client with keyword:{request.String} does not exist"));

            return clients;
        }
    }
}