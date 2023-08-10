using MarketManager.Application.Common.Interfaces;
using System.Security.Claims;

namespace MarketManager.API.Common.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

    }
    public string Username => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
}
