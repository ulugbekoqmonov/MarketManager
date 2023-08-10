﻿using FluentValidation;

namespace MarketManager.Application.UseCases.Roles.Commands.CreateRole;
public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand>
{
    public CreateRoleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MinimumLength(3)
            .MaximumLength(100);

    }
}
