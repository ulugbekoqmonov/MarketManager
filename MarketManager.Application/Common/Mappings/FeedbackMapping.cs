using AutoMapper;
using MarketManager.Application.UseCases.Feedbacks.Commands.CreateFeedback;
using MarketManager.Application.UseCases.Feedbacks.Commands.UpdateFeedback;
using MarketManager.Application.UseCases.Feedbacks.Response;
using MarketManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.Common.Mappings;
public class FeedbackMapping:Profile
{
    public FeedbackMapping()
    {
        CreateMap<FeedbackResponse,Feedback>().ReverseMap();
        CreateMap<CreateFeedbackCommand, Feedback>().ReverseMap();
        CreateMap<UpdateFeedbackCommand, Feedback>().ReverseMap();
    }
}
