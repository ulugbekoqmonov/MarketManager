using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Feedbacks.Commands.CreateFeedback;
using MarketManager.Application.UseCases.Feedbacks.Commands.DeleteFeedback;
using MarketManager.Application.UseCases.Feedbacks.Commands.UpdateFeedback;
using MarketManager.Application.UseCases.Feedbacks.Queries.GetAllFeedbacks;
using MarketManager.Application.UseCases.Feedbacks.Queries.GetByIdFeedback;
using MarketManager.Application.UseCases.Feedbacks.Response;
using MarketManager.Application.UseCases.Users.Queries.GetAllUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class FeedbackController : BaseApiController
{
    [HttpGet("[action]")]
    public async ValueTask<FeedbackResponse> GetFeedbackById(Guid Id)
     =>await _mediator.Send(new GetByIdFeedbackQuery(Id));

    [HttpGet("[action]")]
    public async ValueTask<PaginatedList<FeedbackResponse>> GetAllFeedbacks([FromQuery] GetAllFeedbacksQuery query)
        => await _mediator.Send(query);

    [HttpPost("[action]")]
    public async ValueTask<Guid> CreateFeedback([FromForm] CreateFeedbackCommand command) 
        => await _mediator.Send(command);

    [HttpPut("[action]")]
    public async ValueTask<IActionResult> UpdateFeedback([FromForm] UpdateFeedbackCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("[action]")]
    public async ValueTask<IActionResult> DeleteFeedback([FromForm] DeleteFeedbackCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    


}
