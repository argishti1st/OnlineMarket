using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineMarket.Infrastructure.Data.Identity;
using OnlineMarket.Api.Models;
using OnlineMarket.Api.Helpers;
using OnlineMarket.Api.Configurations;

namespace OnlineMarket.Aapi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            JWTSettings jwtSettings,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserApiModel model)
        {
            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            if (!await _roleManager.RoleExistsAsync(model.Role))
            {
                return BadRequest(new List<IdentityError>
                {
                    new IdentityError { Description = $"Role '{model.Role}' does not exist." }
                });
            }

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginApiModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            var token = JwtHelper.GenerateJwtToken(user, roles, _jwtSettings);

            return Ok(new { Token = token });
        }
    }
}