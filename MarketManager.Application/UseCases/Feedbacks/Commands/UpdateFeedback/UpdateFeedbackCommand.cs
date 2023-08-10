using AutoMapper;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.Feedbacks.Commands.UpdateFeedback;
public class UpdateFeedbackCommand:IRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Name { get; set; }
}
public class UpdateFeedbackCommandHandler : IRequestHandler<UpdateFeedbackCommand>
{
    private readonly IApplicationDbContext _context;

    private readonly IMapper _mapper;
    public UpdateFeedbackCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task Handle(UpdateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var foundFeedback = await _context.Feedbacks.FindAsync(new object[] { request.Id }, cancellationToken);
        if (foundFeedback is null)
            throw new NotFoundException(nameof(Feedback), request.Id);
         foundFeedback = _mapper.Map<Feedback>(request);
        await _context.SaveChangesAsync(cancellationToken);

    }
}
