using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Permissions.ResponseModels;
using MarketManager.Domain.Entities;
using MediatR;

namespace MarketManager.Application.UseCases.Permissions.Queries.GetAllPermissions;

public record GetAllPermissionQuery : IRequest<PaginatedList<PermissionResponse>>
{
    public string? SearchingText { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, PaginatedList<PermissionResponse>>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    public GetAllPermissionQueryHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<PaginatedList<PermissionResponse>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize;
        var pageNumber = request.PageNumber;
        var searchingText = request.SearchingText?.Trim();

        var permissions = _dbContext.Permissions.AsQueryable();

        if (!string.IsNullOrEmpty(searchingText))
        {
            permissions = permissions.Where(p => p.Name.ToLower().Contains(searchingText.ToLower()));
        }
        if (permissions is null || permissions.Count() <= 0)
        {
            throw new NotFoundException(nameof(Permission), searchingText);
        }

        var paginatedPermissions = await PaginatedList<Permission>.CreateAsync(permissions, pageNumber, pageSize);

        var permissionResponses = _mapper.Map<List<PermissionResponse>>(paginatedPermissions.Items);

        var result = new PaginatedList<PermissionResponse>(permissionResponses, paginatedPermissions.TotalCount, request.PageNumber, request.PageSize);
        return result;
    }
}
