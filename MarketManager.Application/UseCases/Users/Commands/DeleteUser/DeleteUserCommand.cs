using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities.Identity;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace MarketManager.Application.UseCases.Users.Commands.DeleteUser;
public record DeleteUserCommand(Guid Id) : IRequest;
public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public DeleteUserCommandHandler(IApplicationDbContext context, IDistributedCache cache, IConfiguration configuration)
    {
        _context = context;
        _cache = cache;
        _configuration = configuration;
    }

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {

        var foundUser = await _context.Users.FindAsync(new object[] { request.Id }, cancellationToken);
        if (foundUser is null)
            throw new NotFoundException(nameof(User), request.Id);


        _context.Users.Remove(foundUser);

        await _context.SaveChangesAsync(cancellationToken);
        await _cache.RemoveAsync(_configuration["RedisKey:User"], cancellationToken);

    }
}
