using AutoMapper;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace MarketManager.Application.UseCases.Feedbacks.Commands.CreateFeedback;
public class CreateFeedbackCommand:IRequest<Guid>
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Name { get; set; }
}
public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand,Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private ITelegramBotClient _botClient;


    public CreateFeedbackCommandHandler(IApplicationDbContext context, IMapper mapper, ITelegramBotClient botClient, IConfiguration configuration)
    {
        _context = context;
        _mapper = mapper;
        _botClient = botClient;
        _configuration = configuration;
    }

    public async Task<Guid> Handle(CreateFeedbackCommand request, CancellationToken cancellationToken)
    {
        var feedback= _mapper.Map<Feedback>(request);
        string messageTemplate = $"Feedback Information\n" +
               $"Title: {feedback.Title}\n" +
               $"Message: {feedback.Message}\n" +
               $"Email: {feedback.Email}\n" +
               $"Phone: {feedback.Phone}\n" +
               $"Name: {feedback.Name}\n" +
               $"Created Date: {feedback.CreatedDate}\n";

        //--write ur teleram id of account to user secrets json-- ok?//
        //start telegrabot for test : https://t.me/ExceptionsPDPbot 
        await _botClient.SendTextMessageAsync(_configuration["TelegramUser:Id"],messageTemplate);
        await _context.Feedbacks.AddAsync(feedback,cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return feedback.Id;
    }
}
