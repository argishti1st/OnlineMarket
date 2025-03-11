using FluentValidation;
using OnlineMarket.Application.Features.Products.Commands;

namespace OnlineMarket.Application.Features.Validators
{
    public class UpdateProductValidator
        : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name must not exceed 50 characters.");
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");
            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Price is required.");
            RuleFor(p => p.Quantity)
                .NotEmpty().WithMessage("Quantity is required.");
            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("Category is required.")
                .MaximumLength(50).WithMessage("Category must not exceed 50 characters.");
        }
    }
}
