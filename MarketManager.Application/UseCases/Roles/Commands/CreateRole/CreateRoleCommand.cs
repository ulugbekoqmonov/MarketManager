using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MarketManager.Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Roles.Commands.CreateRole;
public record CreateRoleCommand : IRequest<Guid>
{
    public string Name { get; set; }
    public List<Guid>? PermissionsIds { get; set; }
}

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;
    public CreateRoleCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var permissions = await _context.Permissions.ToListAsync(cancellationToken);

        var Newpermissons = new List<Permission>();
        if (request.PermissionsIds.Count > 0)
        {
            permissions.ForEach(p =>
            {
                if (request.PermissionsIds.Any(id => p.Id == id))
                    Newpermissons.Add(p);
            });

        }
        var roleEntity = new Role
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Permissions = Newpermissons
        };

        await _context.Roles.AddAsync(roleEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return roleEntity.Id;

    }
}
