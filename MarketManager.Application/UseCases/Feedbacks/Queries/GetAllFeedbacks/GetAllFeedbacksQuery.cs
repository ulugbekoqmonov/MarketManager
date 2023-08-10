using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.Models;
using MarketManager.Application.UseCases.Feedbacks.Response;
using MarketManager.Application.UseCases.Users.Response;
using MarketManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.Feedbacks.Queries.GetAllFeedbacks;
public class GetAllFeedbacksQuery:IRequest<PaginatedList<FeedbackResponse>>
{
    public string? SearchingText { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class GetAllFeedbacksQueryHandler : IRequestHandler<GetAllFeedbacksQuery, PaginatedList<FeedbackResponse>>
{
    private readonly IApplicationDbContext _context;

    private readonly IMapper _mapper;
    public GetAllFeedbacksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<PaginatedList<FeedbackResponse>> Handle(GetAllFeedbacksQuery request, CancellationToken cancellationToken)
    {
        var pageSize = request.PageSize;
        var pageNumber = request.PageNumber;
        var searchingText = request.SearchingText?.Trim();

        IQueryable<Feedback> feedbacks =null;


        if (!string.IsNullOrEmpty(searchingText))
        {
            feedbacks = _context.Feedbacks
                .Where(u => u.Title.ToLower().Contains(searchingText.ToLower())
                || u.Message.ToLower().Contains(searchingText.ToLower())
                || u.Phone.ToLower().Contains(searchingText.ToLower())
                || u.Email.ToLower().Contains(searchingText.ToLower())
                || u.Name.ToLower().Contains(searchingText.ToLower()));
        }
        else
        {
            feedbacks = _context.Feedbacks.AsQueryable();
        }
        var paginatedFeedback = await PaginatedList<Feedback>.CreateAsync(feedbacks, pageNumber, pageSize);
        var responseFeedbacks = _mapper.Map<List<FeedbackResponse>>(paginatedFeedback.Items);
        var result = new PaginatedList<FeedbackResponse>
            (responseFeedbacks, paginatedFeedback.TotalCount, request.PageNumber, request.PageSize);
        return result;
    }
}
