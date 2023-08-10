using FluentValidation;

namespace MarketManager.Application.UseCases.Permissions.Commands.UpdatePermission;

public class UpdatePermissionCommandValidation : AbstractValidator<UpdatePermissionCommand>
{

    public UpdatePermissionCommandValidation()
    {
        RuleFor(x => x.Name)
           .NotNull()
           .NotEmpty().WithMessage("Permission name must not be empty!")
           .MaximumLength(255).WithMessage("Permission name cannot exceed 255 characters.");
    }

}
