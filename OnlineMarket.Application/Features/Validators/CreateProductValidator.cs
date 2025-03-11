using FluentValidation;
using OnlineMarket.Application.Features.Products.Commands;

namespace OnlineMarket.Application.Features.Validators
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
            RuleFor(p => p.Description)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(500).WithMessage("{PropertyName} must not exceed 200 characters.");
            RuleFor(p => p.Price)
                .NotEmpty()
                .GreaterThan(0).WithMessage("{PropertyName} is required and must be greater than 0.");
            RuleFor(p => p.Quantity)
                .NotEmpty()
                .GreaterThan(0).WithMessage("{PropertyName} is required and must be greater than 0.");
            RuleFor(p => p.Category)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");
        }
    }
}
