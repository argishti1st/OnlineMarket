using Elastic.CommonSchema;
using Microsoft.IdentityModel.Tokens;
using OnlineMarket.Api.Configurations;
using OnlineMarket.Infrastructure.Data.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OnlineMarket.Api.Helpers
{
    public static class JwtHelper
    {
        public static string GenerateJwtToken(ApplicationUser user, IList<string> roles, JWTSettings jwt)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            // Add role claims
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var token = new JwtSecurityToken(
                jwt.Issuer,
                jwt.Audience,
                claims,
                expires: DateTime.UtcNow.AddMinutes(jwt.ExpirationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
