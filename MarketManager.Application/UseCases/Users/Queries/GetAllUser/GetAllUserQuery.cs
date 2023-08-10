using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Users.Response;
using MarketManager.Domain.Entities;
using MarketManager.Domain.Entities.Identity;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace MarketManager.Application.UseCases.Users.Queries.GetAllUser;
public record GetAllUserQuery : IRequest<PaginatedList<UserResponse>>
{
    public string? SearchingText { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, PaginatedList<UserResponse>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private readonly IConfiguration _configuration;

    public GetAllUserQueryHandler(IApplicationDbContext context, IMapper mapper, IDistributedCache cache, IConfiguration configuration)
    {
        (_context, _mapper) = (context, mapper);
        _cache = cache;
        _configuration = configuration;
    }

    public async Task<PaginatedList<UserResponse>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize;
        var pageNumber = request.PageNumber;
        var searchingText = request.SearchingText?.Trim();

        List<User> users=new();
        var settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        string? serializedData = null;
        var dataAsByteArray = await _cache.GetAsync(_configuration["RedisKey:User"], cancellationToken);
         
        if ((dataAsByteArray?.Count() ?? 0) > 0)
        {
            serializedData = Encoding.UTF8.GetString(dataAsByteArray);
            users = JsonConvert.DeserializeObject
                <List<User>>(serializedData,settings);
        }
        else
        {
               
            users = await _context.Users.ToListAsync(cancellationToken);
               
            serializedData = JsonConvert.SerializeObject(users, settings);
            dataAsByteArray = Encoding.UTF8.GetBytes(serializedData);
            await _cache.SetAsync(_configuration["RedisKey:User"], dataAsByteArray,cancellationToken);
        }
        
      



        if (!string.IsNullOrEmpty(searchingText))
        {
            users = users
                .Where(u => u.Username.ToLower().Contains(searchingText.ToLower())
                || u.FullName.ToLower().Contains(searchingText.ToLower()) 
                || u.Phone.ToLower().Contains(searchingText.ToLower())).ToList();
        }
        var paginatedUser = await PaginatedList<User>.CreateAsync(users, pageNumber, pageSize);
        var responseUser = _mapper.Map<List<UserResponse>>(paginatedUser.Items);
        var result = new PaginatedList<UserResponse>
            (responseUser,paginatedUser.TotalCount, request.PageNumber,request.PageSize);
        return result;
    }
}


