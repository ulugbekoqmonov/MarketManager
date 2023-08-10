using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.UseCases.Feedbacks.Response;
using MarketManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.Feedbacks.Queries.GetByIdFeedback;
public record GetByIdFeedbackQuery(Guid Id):IRequest<FeedbackResponse>;


public class GetByIdFeedbackQueryHandler : IRequestHandler<GetByIdFeedbackQuery, FeedbackResponse>
{
    private readonly IApplicationDbContext _context;

    private readonly IMapper _mapper;
    public GetByIdFeedbackQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<FeedbackResponse> Handle(GetByIdFeedbackQuery request, CancellationToken cancellationToken)
    {
        var foundFeedback = await _context.Feedbacks.FindAsync(new object[] {request.Id},cancellationToken);
        if (foundFeedback is null)
            throw new NotFoundException(nameof(Feedback), request.Id);

        var response  = _mapper.Map<FeedbackResponse>(foundFeedback);   

        return response;
    }
}
