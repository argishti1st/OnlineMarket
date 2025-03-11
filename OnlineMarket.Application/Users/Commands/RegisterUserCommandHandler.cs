using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineMarket.Domain.Common;
using OnlineMarket.Domain.Entities.Identity;

namespace OnlineMarket.Application.Users.Commands
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<IEnumerable<IdentityError>>>
    {
        public RegisterUserCommandHandler()
        {
        }

        public async Task<Result<IEnumerable<IdentityError>>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var user = new ApplicationUser { UserName = request.Username, Email = request.Email };
            //var result = await _userManager.CreateAsync(user, request.Password);

            //if (!result.Succeeded)
            //{
            //    return Result<IEnumerable<IdentityError>>.Failure(result.Errors, false);
            //}

            //if (!await _roleManager.RoleExistsAsync(request.Role))
            //{
            //    return Result<IEnumerable<IdentityError>>.Failure(new List<IdentityError>
            //    {
            //        new IdentityError { Description = $"Role '{request.Role}' does not exist." }
            //    },
            //    false);
            //}

            //await _userManager.AddToRoleAsync(user, request.Role);
            return Result<IEnumerable<IdentityError>>.Success(Enumerable.Empty<IdentityError>());
        }
    }
}
