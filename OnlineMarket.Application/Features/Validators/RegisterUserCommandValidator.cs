using FluentValidation;
using OnlineMarket.Application.Users.Commands;

namespace OnlineMarket.Application.Features.Validators
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.Username)
                .NotEmpty();
            RuleFor(x => x.Password)
                .NotEmpty()
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$")
                .MinimumLength(6);
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => role == "Admin" || role == "User")
                .WithMessage("Role must be either 'Admin' or 'User'.");
        }
    }
}
