using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Users.Response;
using MarketManager.Domain.Entities.Identity;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.Users.Filters;
public record GetFilteredUsers : IRequest<PaginatedList<UserResponse>>
{
    public DateOnly? StartDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

}
public class GetFilteredUsersHandler : IRequestHandler<GetFilteredUsers, PaginatedList<UserResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetFilteredUsersHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<UserResponse>> Handle(GetFilteredUsers request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize;
        var pageNumber = request.PageNumber;
        var users = _context.Users.AsQueryable();
        if (request.EndDate is null)
            request.EndDate = DateOnly.FromDateTime(DateTime.Now);

        if (!request.StartDate.HasValue)
        {
           users= users.Where(date => DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate);
            return await GetPaginated(request, pageSize, pageNumber, users);
        }
        else
        {
           users= users.Where(date => DateOnly.FromDateTime(date.CreatedDate) >= request.StartDate
            && DateOnly.FromDateTime(date.CreatedDate) <= request.EndDate);
            return await GetPaginated(request, pageSize, pageNumber, users);
        }


    }

    private async Task<PaginatedList<UserResponse>> GetPaginated(GetFilteredUsers request, int pageSize, int pageNumber, IQueryable<User> users)
    {
        var paginatedUser = await PaginatedList<User>.CreateAsync(users, pageNumber, pageSize);
        var responseUser = _mapper.Map<List<UserResponse>>(paginatedUser.Items);
        var result = new PaginatedList<UserResponse>
            (responseUser, paginatedUser.TotalCount, request.PageNumber, request.PageSize);
        return result;
    }
}
