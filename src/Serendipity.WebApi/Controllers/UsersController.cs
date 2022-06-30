using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serendipity.Domain.Defaults;
using Serendipity.Infrastructure.Models;
using Serendipity.WebApi.Contracts;
using Serendipity.WebApi.Contracts.Requests;
using Serendipity.WebApi.Contracts.Responses;
using Serendipity.WebApi.Filters;

namespace Serendipity.WebApi.Controllers;


[Route("api/v1/[controller]")]
[ApiController]
[ServiceFilter(typeof(InputValidationActionFilter))]
public class UsersController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;

    public UsersController(
        UserManager<User> userManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.Users
            .Where(u => u.Email == model.Email)
            .Include(u => u.PersonalInfo)
            .SingleOrDefaultAsync();
        if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password)) return Unauthorized();
        
        var userRoles = await _userManager.GetRolesAsync(user);

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };
        
        authClaims.AddRange(
            userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole))
            );

        var token = GetToken(authClaims);

        return Ok(new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = token.ValidTo,
            User = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Weight = user.PersonalInfo?.Weight,
                Height = user.PersonalInfo?.Height,
                BirthDay = user.PersonalInfo?.BirthDay,
                Roles = userRoles
            }
        });
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest userRequest)
    {
        var userExists = await _userManager.FindByEmailAsync(userRequest.Email);
        if (userExists != null)
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = "User already exists!" });
            
        
        User user = new()
        {
            Email = userRequest.Email,
            UserName = userRequest.Email,
            Name = userRequest.Name,
            Surname = userRequest.Surname,
            SecurityStamp = Guid.NewGuid().ToString(),
            PersonalInfo = new PersonalInfo
            {
                BirthDay = userRequest.DayOfBirth,
                Weight = userRequest.Weight,
                Height = userRequest.Height,
                Job = userRequest.Job
            }
        };
        var result = await _userManager.CreateAsync(user, userRequest.Password);
        if (!result.Succeeded)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                new Response
                {
                    Status = "Error", 
                    Message = "User creation failed! Please check user details and try again."
                });
        }

        return Ok();
    }
    
    [HttpPost]
    [Route("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateUserRequest userRequest)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        

        user.Email = userRequest.Email;
        user.Name = userRequest.Name;
        user.Surname = userRequest.Surname;
        user.PersonalInfo = new PersonalInfo
        {
            Height = userRequest.Height,
            Weight = userRequest.Weight,
            Job = userRequest.Job,
            BirthDay = userRequest.DayOfBirth
        };
        
        
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Admin updating failed. Please check your info and try again.");
        }

        return Ok();
    }

    [HttpPost]
    [Route("password-reset")]
    public async Task<IActionResult> PasswordReset([FromBody] PasswordResetRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return Ok();
        
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        
        //TODO: use SNS to send email with token
        return Ok();
    }
    [HttpPost]
    [Route("confirm-password-reset")]
    public async Task<IActionResult> PasswordResetPost([FromBody] PasswordResetConfirmationRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return NotFound($"{request.Email} is not registered.");
        }

        var res = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (!res.Succeeded)
        {
            return StatusCode(500, "Reset failed.");
        }

        return Ok();
    }
    private JwtSecurityToken GetToken(IEnumerable<Claim> authClaims)
    {
        string secret = _configuration["JWT:Secret"] ??
                        throw new Exception("JWT:Secret missing from appsettings.json.");
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"] ?? throw new Exception("JWT:ValidIssuer missing from appsettings.json."),
            audience: _configuration["JWT:ValidAudience"] ?? throw new Exception("JWT:ValidAudience missing from appsettings.json."),
            expires: DateTime.Now.AddHours(24),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

        return token;
    }
}