using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(
            UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository
        ) // this is available because of the services.AddIdentityCore<IdentityUser>() in Program.cs
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            if (registerRequestDto == null)
            {
                return BadRequest();
            }

            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username,
            };

            var identityResult = await userManager.CreateAsync(
                identityUser,
                registerRequestDto.Password
            );

            if (identityResult.Succeeded)
            {
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(
                        identityUser,
                        registerRequestDto.Roles
                    );

                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered. Please login");
                    }
                }
            }

            return BadRequest(identityResult.Errors);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            if (loginRequestDto == null)
            {
                return BadRequest();
            }
            var identityUser = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (identityUser != null)
            {
                var identityResult = await userManager.CheckPasswordAsync(
                    identityUser,
                    loginRequestDto.Password
                );
                if (identityResult)
                {
                    // create token
                    var userRoles = await userManager.GetRolesAsync(identityUser);
                    if (userRoles.Any())
                    {
                        var jwtToken = tokenRepository.GenerateJWTToken(
                            identityUser,
                            userRoles.ToList()
                        );
                        var response = new LoginResponseDto { JwtToken = jwtToken };
                        return Ok(response);
                    }
                }
            }

            return BadRequest("Invalid username or password");
        }
    }
}
