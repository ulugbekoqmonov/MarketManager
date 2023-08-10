using MarketManager.Application.Common.JWT.Models;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Users.Commands.CreateUser;
using MarketManager.Application.UseCases.Users.Commands.DeleteUser;
using MarketManager.Application.UseCases.Users.Commands.LoginUser;
using MarketManager.Application.UseCases.Users.Commands.RegisterUser;
using MarketManager.Application.UseCases.Users.Commands.UpdateUser;
using MarketManager.Application.UseCases.Users.Filters;
using MarketManager.Application.UseCases.Users.Queries.GetAllUser;
using MarketManager.Application.UseCases.Users.Queries.GetByIdUser;
using MarketManager.Application.UseCases.Users.Response;
using Microsoft.AspNetCore.Mvc;

namespace MarketManager.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : BaseApiController
{

    [HttpGet("[action]")]
    public async ValueTask<UserResponse> GetUserById(Guid userId)
        => await _mediator.Send(new GetByIdUserQuery(userId));

    [HttpGet("[action]")]
    public async ValueTask<PaginatedList<UserResponse>> GetAllUser([FromQuery]GetAllUserQuery query)
           => await _mediator.Send(query);


    [HttpGet("[action]")]
    public async ValueTask<PaginatedList<UserResponse>> GetFilteredUsers([FromQuery]GetFilteredUsers query)
           => await _mediator.Send(query);


    [HttpPost("[action]")]
    public async ValueTask<TokenResponse> RegisterUser([FromForm]RegisterUserCommand command)
        => await _mediator.Send(command);


    [HttpPost("[action]")]
    public async ValueTask<TokenResponse> LoginUser([FromForm] LoginUserCommand command)
        => await _mediator.Send(command);

    [HttpPost("[action]")]
    public async ValueTask<Guid> CreateUser([FromForm] CreateUserCommand command)
        => await _mediator.Send(command);


    [HttpPut("[action]")]
    public async ValueTask<IActionResult> UpdateUser([FromForm] UpdateUserCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }


    [HttpDelete("[action]")]
    public async ValueTask<IActionResult> DeleteUser([FromForm] DeleteUserCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }


}
