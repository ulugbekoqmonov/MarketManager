using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.JWT.Interfaces;
using MarketManager.Application.UseCases.Users.Commands.LoginUser;
using MarketManager.Application.UseCases.Users.Response;
using MarketManager.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.Common.JWT.Service;
public class RefreshToken : IUserRefreshToken
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public RefreshToken(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async ValueTask<UserRefreshToken> AddOrUpdateRefreshToken(UserRefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        var foundRefreshtoken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserName == refreshToken.UserName, cancellationToken);
        if (foundRefreshtoken is null)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return refreshToken;
        }
        else
        {
            foundRefreshtoken.RefreshToken = refreshToken.RefreshToken;
            foundRefreshtoken.ExpiresTime = refreshToken.ExpiresTime;
            _context.RefreshTokens.Update(foundRefreshtoken);
            await _context.SaveChangesAsync(cancellationToken);
            return refreshToken;
        }
    }

    public async ValueTask<User> AuthenAsync(LoginUserCommand user)
    {
        string hashPassword = user.Password.GetHashedString();
        User? foundUser = await _context.Users.SingleOrDefaultAsync(x => x.Username == user.Username && x.Password == hashPassword);
        if (foundUser is null)
        {
            return null;
        }

        

        return foundUser;
    }

    public async ValueTask<bool> DeleteUserRefreshTokens(string username, string refreshToken, CancellationToken cancellationToken = default)
    {
        var foundRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken);
        _context.RefreshTokens.Remove(foundRefreshToken);
        return (await _context.SaveChangesAsync(cancellationToken)) > 0;
    }

    public async ValueTask<UserRefreshToken> GetSavedRefreshTokens(string username, string refreshtoken)
    {
        var foundRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshtoken);
        return foundRefreshToken;
    }
}
