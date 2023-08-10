using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketManager.Application.UseCases.Cashbacks.Commands.DeleteCashback
{
    public class DeleteCashbackCommandValidator:AbstractValidator<DeleteCashbackCommand>
    {
        public DeleteCashbackCommandValidator()
        {
              RuleFor(cashback=> cashback.Id).NotEmpty();
        }
    }
}
