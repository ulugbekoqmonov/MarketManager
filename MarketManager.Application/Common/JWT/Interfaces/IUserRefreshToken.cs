using MarketManager.Application.UseCases.Users.Commands.LoginUser;
using MarketManager.Application.UseCases.Users.Response;
using MarketManager.Domain.Entities.Identity;

namespace MarketManager.Application.Common.JWT.Interfaces;
public interface IUserRefreshToken
{
    ValueTask<UserRefreshToken> AddOrUpdateRefreshToken(UserRefreshToken refreshToken, CancellationToken cancellationToken = default);
    ValueTask<User> AuthenAsync(LoginUserCommand user);
    ValueTask<bool> DeleteUserRefreshTokens(string username, string refreshToken, CancellationToken cancellationToken = default);
    ValueTask<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshtoken);
}
