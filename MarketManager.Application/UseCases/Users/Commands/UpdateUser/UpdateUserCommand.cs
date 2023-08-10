using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace MarketManager.Application.UseCases.Users.Commands.UpdateUser;
public class UpdateUserCommand : IRequest
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Username { get; set; }
    public string? Password { get; set; }
    public Guid[]? RoleIds { get; set; }
    public IFormFile? ProfilePicture { get; set; }
}
public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IDistributedCache _cache;
    public UpdateUserCommandHandler(IApplicationDbContext context, IConfiguration configuration, IDistributedCache cache)
    {
        (_context, _configuration) = (context, configuration);
        _cache = cache;
    }

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var roles = await _context.Roles.ToListAsync(cancellationToken);
        var foundUser = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
        if (foundUser is null)
            throw new NotFoundException(nameof(User), request.Id);


       
        if (request?.RoleIds?.Length > 0)
        {
            foundUser?.Roles?.Clear();
            roles.ForEach(role =>
            {
                if (request.RoleIds.Any(id => id == role.Id))
                    foundUser.Roles.Add(role);
            });

        }


        foundUser.Username = request.Username;
        if (!string.IsNullOrEmpty(request.Password))
            foundUser.Password = request.Password.GetHashedString();

        foundUser.Phone = request.Phone;
        foundUser.FullName = request.FullName;
        if (request.ProfilePicture != null)
        {
            var picturepath = _configuration["UserPicturePath"];
            string filename = foundUser.Username + Path.GetExtension(request.ProfilePicture.FileName);
            var userImagePath = Path.Combine(picturepath, filename);

            using (var fs = new FileStream(userImagePath, FileMode.Create))
            {
                await request.ProfilePicture.CopyToAsync(fs);
                foundUser.Picture = userImagePath;
            }
        }


        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(_configuration["RedisKey:User"], cancellationToken);

    }
}
