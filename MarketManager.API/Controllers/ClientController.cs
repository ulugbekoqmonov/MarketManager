using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Clients.Commands.CreateClient;
using MarketManager.Application.UseCases.Clients.Commands.DeleteClient;
using MarketManager.Application.UseCases.Clients.Commands.UpdateClient;
using MarketManager.Application.UseCases.Clients.Filters;
using MarketManager.Application.UseCases.Clients.Queries.GetAllClients;
using MarketManager.Application.UseCases.Clients.Queries.GetClientById;
using MarketManager.Application.UseCases.Clients.Reports;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : BaseApiController
    {
        [HttpGet("[action]")]
        public async ValueTask<ActionResult<PaginatedList<GetAllClientsQueryResponse>>> GetAllClients([FromQuery] GetAllClientsQuery query)
        {
            return await _mediator.Send(query);
        }
        [HttpGet("[action]")]
        public async Task<ActionResult<PaginatedList<GetAllClientsQueryResponse>>> GetFilteredClients(
        [FromQuery] ClientsFilterQuery filterCommand)
        {
            return await _mediator.Send(filterCommand);

        }

        [HttpGet("[action]")]
        public async ValueTask<GetClientByIdQueryResponse> GetClientById(Guid Id)
        {
            return await _mediator.Send(new GetClientByIdQuery(Id));
        }

        [HttpPost("[action]")]
        public async ValueTask<Guid> CreateClient(CreateClientCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("[action]")]
        public async ValueTask<IActionResult> UpdateClient(UpdateClientCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async ValueTask<IActionResult> DeleteClient(Guid Id)
        {
            await _mediator.Send(new DeleteClientCommand(Id));
            return NoContent();
        }
        [HttpGet("[action]")]
        public async Task<FileResult> ExportExcelClients(string fileName = "clients")
        {
            var result = await _mediator.Send(new ClientExportExcel { FileName = fileName });
            return File(result.FileContents, result.Option, result.FileName);
        }
        [HttpPost("[action]")]
        public async Task<List<ClientsResponseExcelReport>> ImportExcelClients(IFormFile excelfile)
        {
            List<ClientsResponseExcelReport> result = await _mediator.Send(new AddClientFromExcel(excelfile));
            return result;
        }
    }
}

