using FluentValidation;

namespace MarketManager.Application.UseCases.Permissions.Commands.CreatePermission
{
    public class CreatePermissionCommandValidation : AbstractValidator<CreatePermissionCommand>
    {
        public CreatePermissionCommandValidation()
        {
            RuleFor(x => x.Name)
                .Must(BeUniqueNames).WithMessage("Permission names must be unique.")
                .ForEach(name =>
                {
                    name.NotEmpty().WithMessage("Permission name must not be empty!")
                        .MaximumLength(255).WithMessage("Permission name cannot exceed 255 characters.");
                });
        }
        private bool BeUniqueNames(string[] names)
        {
            // Implement your logic to check if all names in the array are unique.
            // You can use LINQ's Distinct() method to check for uniqueness.

            // Example (assuming you have a helper method to check uniqueness):
            return names.Distinct().Count() == names.Length;
        }
    }

}
