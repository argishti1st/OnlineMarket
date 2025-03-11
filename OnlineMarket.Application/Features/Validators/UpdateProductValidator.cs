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
                .NotEmpty()
                .WithMessage("Name is required.")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters.");

            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Description is required.")
                .MaximumLength(200)
                .WithMessage("Description must not exceed 200 characters.");

            RuleFor(p => p.Price)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is required and must be greater than 0.");

            RuleFor(p => p.Quantity)
                .NotEmpty()
                .GreaterThan(0)
                .WithMessage("{PropertyName} is required and must be greater than 0.");

            RuleFor(p => p.Category)
                .NotEmpty()
                .WithMessage("{PropertyName} is required.")
                .MaximumLength(50)
                .WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}
