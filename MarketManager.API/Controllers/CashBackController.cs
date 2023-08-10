using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Cashbacks.Commands.CreateCashback;
using MarketManager.Application.UseCases.Cashbacks.Commands.DeleteCashback;
using MarketManager.Application.UseCases.Cashbacks.Commands.UpdateCashback;
using MarketManager.Application.UseCases.Cashbacks.Queries.GetAllCashbacks;
using MarketManager.Application.UseCases.Cashbacks.Queries.GetCashbackById;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashbackController : BaseApiController
    {
        [HttpGet("[action]")]
        public async ValueTask<ActionResult<PaginatedList<GetAllCashbacksQueryResponse>>> GetAllCashbacks([FromQuery] GetAllCashbacksQuery query)
        {
            return await _mediator.Send(query);
        }
       
        [HttpGet("[action]")]
        public async ValueTask<GetCashbackByIdQueryResponse> GetCashbackById(Guid Id)
        {
            return await _mediator.Send(new GetCashbackByIdQuery(Id));
        }

        [HttpPost("[action]")]
        public async ValueTask<Guid> CreateCashback(CreateCashbackCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("[action]")]
        public async ValueTask<IActionResult> UpdateCashback(UpdateCashbackCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("[action]")]
        public async ValueTask<IActionResult> DeleteCashback(DeleteCashbackCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
