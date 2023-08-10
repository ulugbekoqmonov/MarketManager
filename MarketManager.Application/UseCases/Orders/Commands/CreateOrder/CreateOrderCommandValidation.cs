﻿using FluentValidation;

namespace MarketManager.Application.UseCases.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidation : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidation()
        {
            RuleFor(x => x.TotalPrice).GreaterThanOrEqualTo(0)
                .WithMessage(" total price can't be lower than zero. ");

        }
    }
}
