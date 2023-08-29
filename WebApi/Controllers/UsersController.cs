using Domain.DTOs;
using Entities;
using Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Token;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public UsersController(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 400)]
    [ProducesResponseType(typeof(string), 500)]
    [HttpPost("register")]
    public async Task<IActionResult> Register(CreateUserDto createUserDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createUserDto.Email))
                return Unauthorized("No email sent.");

            if (string.IsNullOrWhiteSpace(createUserDto.Password))
                return Unauthorized("No password sent.");

            var user = new User
            {
                UserName = createUserDto.Email,
                Email = createUserDto.Email,
                AccessLevel = AccessLevel.Common
            };

            IdentityResult result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (result.Errors.Any())
            {
                return BadRequest(result.Errors);
            }

            return Ok($"User created");
        }
        catch (Exception ex)
        {
            return Problem(ex.ToString());
        }
    }

    [AllowAnonymous]
    [ProducesResponseType(typeof(string), 200)]
    [ProducesResponseType(typeof(string), 401)]
    [ProducesResponseType(typeof(string), 500)]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(loginUserDto.Email))
                return Unauthorized("No email sent.");

            if (string.IsNullOrWhiteSpace(loginUserDto.Password))
                return Unauthorized("No password sent.");

            SignInResult result = await _signInManager.PasswordSignInAsync(
                loginUserDto.Email,
                loginUserDto.Password,
                false,
                false);

            if (result.Succeeded)
            {
                var user = _userManager.Users
                    .FirstOrDefault(u => u.NormalizedEmail == loginUserDto.Email.ToUpper());

                if (user == null)
                {
                    return NotFound("O usuário não foi encontrado.");
                }

                var token = new TokenJWTBuilder()
                    .AddSecurityKey(JwtSecurityKey.Create("948IFIF4949FKFK4949FK1"))
                    .AddSubject("AspNetCore")
                    .AddIssuer("AspNetCore")
                    .AddAudience("AspNetCore")
                    .AddClaim("id", user.Id)
                    .AddClaim("timestamp", DateTime.UtcNow.ToString())
                    .SetExpiresIn(5)
                    .Builder();

                return Ok(token.Value);
            }
            else
            {
                return Unauthorized($"User not authorized: {result}");
            }
        }
        catch (Exception ex)
        {
            return Problem(ex.ToString());
        }
    }
}
