using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Roles.Commands.UpdateRole;
public class UpdateRoleCommand : IRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Guid>? PermissionsIds { get; set; }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateRoleCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var permissions = await _context.Permissions.ToListAsync(cancellationToken);
        var entity = await _context.Roles.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity is null)
            throw new NotFoundException(nameof(Role), request.Id);

        if (request?.PermissionsIds?.Count > 0)
        {
            entity.Permissions.Clear();
            permissions.ForEach(p =>
            {
                if (request.PermissionsIds.Any(id => p.Id == id))
                    entity.Permissions.Add(p);
            });

        }
        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
