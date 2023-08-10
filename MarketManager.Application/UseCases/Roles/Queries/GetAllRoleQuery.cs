using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Roles.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MarketManager.Application.UseCases.Roles.Queries;
public record GetAllRoleQuery : IRequest<List<RoleResponse>>
{
}

public class GetAllRoleQueryHandler : IRequestHandler<GetAllRoleQuery, List<RoleResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public GetAllRoleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<RoleResponse>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
    {
        var entities = await _context.Roles.ToListAsync(cancellationToken);
        var result = _mapper.Map<List<RoleResponse>>(entities);
        return result;
    }
}

