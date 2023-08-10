using MarketManager.Application.Common.Interfaces;
using MediatR;

namespace MarketManager.Application.UseCases.Permissions.Commands.DeletePermission
{
    public record DeletePermissionCommand(Guid PermissionId) : IRequest;
    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand>
    {
        private readonly IApplicationDbContext _context;

        public DeletePermissionCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeletePermissionCommand request, CancellationToken cancellationToken)
        {
            var permission = await _context.Permissions.FindAsync(request.PermissionId, cancellationToken);
            if (permission is null)
            {
                throw new NotFoundException(nameof(Permissions), request.PermissionId);
            }
            _context.Permissions.Remove(permission);
            var result = await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
