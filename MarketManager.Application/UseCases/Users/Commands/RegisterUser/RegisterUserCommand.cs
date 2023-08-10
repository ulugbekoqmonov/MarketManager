using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.JWT.Interfaces;
using MarketManager.Application.Common.JWT.Models;
using MarketManager.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace MarketManager.Application.UseCases.Users.Commands.RegisterUser;
public class RegisterUserCommand : IRequest<TokenResponse>
{
    public string FullName { get; set; }
    public string Username { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public IFormFile? ProfilePicture { get; set; }
}
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, TokenResponse>
{
    private readonly IJwtToken _jwtToken;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;


    public RegisterUserCommandHandler(IJwtToken jwtToken, IApplicationDbContext context, IMapper mapper, IConfiguration configuration, IDistributedCache cache)
    {
        _jwtToken = jwtToken;
        _context = context;
        _mapper = mapper;
        _configuration = configuration;
        _cache = cache;
    }
    public async Task<TokenResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

        if (_context.Users.Any(x => x.Username == request.Username))
            throw new AlreadyExistsException(nameof(User), request.Username);


        var user = _mapper.Map<User>(request);
        user.Password = user.Password.GetHashedString();
        if (request.ProfilePicture != null)
        {
            var picturepath = _configuration["UserPicturePath"];
            string filename = user.Username + Path.GetExtension(request.ProfilePicture.FileName);
            var userImagePath = Path.Combine(picturepath, filename);

            using (var fs = new FileStream(userImagePath, FileMode.Create))
            {
                await request.ProfilePicture.CopyToAsync(fs);
                user.Picture = userImagePath;
            }
        }

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        var tokenResponse = await _jwtToken.CreateTokenAsync(user.Username, user.Id.ToString(), new List<Role>(), cancellationToken);
        await _cache.RemoveAsync(_configuration["RedisKey:User"], cancellationToken);
        return tokenResponse;
    
    }
}
