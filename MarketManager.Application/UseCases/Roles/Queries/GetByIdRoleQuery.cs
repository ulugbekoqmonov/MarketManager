using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Roles.Response;
using MarketManager.Domain.Entities.Identity;
using MediatR;

namespace MarketManager.Application.UseCases.Roles.Queries;
public record GetByIdRoleQuery : IRequest<RoleResponse>
{
    public Guid Id { get; set; }
}

public class GetByIdRoleQueryHandler : IRequestHandler<GetByIdRoleQuery, RoleResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetByIdRoleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RoleResponse> Handle(GetByIdRoleQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Roles.FindAsync(new object[] { request.Id }, cancellationToken);
        if (entity is null)
            throw new NotFoundException(nameof(Role), request.Id);

        var result = _mapper.Map<RoleResponse>(entity);
        return result;
    }
}
