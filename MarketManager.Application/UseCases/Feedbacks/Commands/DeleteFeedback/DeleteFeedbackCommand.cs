using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.Feedbacks.Commands.DeleteFeedback;
public class DeleteFeedbackCommand:IRequest
{
    public Guid Id { get; set; }
}
public class DeleteFeedbackCommandHandler : IRequestHandler<DeleteFeedbackCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteFeedbackCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteFeedbackCommand request, CancellationToken cancellationToken)
    {
      var foundfeedback = await  _context.Feedbacks.FindAsync(new object[] { request.Id },cancellationToken);
        if (foundfeedback is null)
            throw new NotFoundException(nameof(Feedback), request.Id);
        _context.Feedbacks.Remove(foundfeedback);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
