using MediatR;
using Microsoft.AspNetCore.Identity;
using OnlineMarket.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarket.Application.Users.Commands
{
    public record RegisterUserCommand(
        string Email,
        string Username,
        string Password,
        string Role) : IRequest<Result<IEnumerable<IdentityError>>>;
}
