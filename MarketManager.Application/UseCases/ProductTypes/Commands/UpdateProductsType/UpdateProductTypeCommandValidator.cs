using FluentValidation;
using MarketManager.Application.UseCases.ProductsType.Commands.UpdateProductType;
using Microsoft.AspNetCore.Http;

namespace MarketManager.Application.UseCases.ProductTypes.Commands.UpdateProductsType;
public class UpdateProductTypeCommandValidator : AbstractValidator<UpdateProductTypeCommand>
{
    public UpdateProductTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MinimumLength(3).WithMessage("Name must not be less than 3.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Picture)
            .Must(HaveValidPictureExtension).WithMessage("Invalid picture format.")
            .When(x => x.Picture != null);
    }
    private bool HaveValidPictureExtension(IFormFile picture)
    {
        var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(picture.FileName).ToLowerInvariant();
        return validExtensions.Contains(extension);
    }
}
