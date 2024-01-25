using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Application.Clients.Command.CreateClient;
using Application.Clients.Command.UpdateClient;
using Application.Clients.Query;
using Application.Common.Dtos;

namespace API.Controllers.V1
{
    public class ClientsController : BaseController
    {
        /// <summary>
        /// Create Client
        /// </summary>
        /// <remarks>
        /// Create new client <br/>
        /// Send an email and sync documents after a client is created (using the mock repositories provided)
        /// </remarks>
        /// <response code="200">Returns a JSON object of Client</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPost]
        public async Task<ActionResult<ClientDto>> CreateClient([FromQuery] CreateClientCommand command)
        {
            var client = await Mediator.Send(command);
            return Ok(client);
        }

        /// <summary>
        /// Update Client
        /// </summary>
        /// <remarks>
        /// Update existing client <br/>
        /// If the email has changed, send an email and sync documents after a client is updated (using the mock repositories provided)
        /// </remarks>
        /// <response code="200">Returns a JSON object of Client</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<ClientDto>> UpdateClient([FromRoute] long id, [FromBody] UpdateClientCommand command)
        {
            command.Id = id;
            var client = await Mediator.Send(command);

            return Ok(client);
        }

        /// <summary>
        /// Search for Client
        /// </summary>
        /// <remarks>
        /// Retrieves a client <br/>
        /// Searching in the "search field" should filter the list of clients by their first or last name <br/>
        /// Example: John Stevens and Steven Smith should both show up if a user searches "steven"
        /// </remarks>
        /// <response code="200">Returns a JSON object of client</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet("Search")]
        public async Task<ActionResult<List<ClientDto>>> FindClient([FromQuery] GetClientQuery query)
        {
            var clients = await Mediator.Send(query);

            return Ok(clients);
        }
    }
}
