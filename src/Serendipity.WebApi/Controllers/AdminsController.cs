using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Serendipity.Domain.Defaults;
using Serendipity.Infrastructure.Models;
using Serendipity.WebApi.Contracts;
using Serendipity.WebApi.Contracts.Requests;
using Serendipity.WebApi.Filters;

namespace Serendipity.WebApi.Controllers;

[Authorize(Roles = Roles.Admin)]
[Route("api/v1/[controller]")]
[ServiceFilter(typeof(InputValidationActionFilter))]
[ApiController]
public class AdminsController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    // GET
    public AdminsController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    [HttpGet]
    [Route("/")]
    public async Task<IActionResult> GetAll()
    {
        var admins = await _userManager.GetUsersInRoleAsync(Roles.Admin) ?? new List<User>();

        return Ok(admins.Select(a => new { a.Id, a.Email}));
    }

    [HttpGet]
    [Route("/{userId}")]
    public async Task<IActionResult> Get(string userId)
    {
        var admin = await _userManager.FindByIdAsync(userId);

        if (admin is null) return NotFound();

        return Ok(admin);
    }

    [HttpPost]
    [Route("/")]
    public async Task<IActionResult> Insert([FromBody] RegisterAdminRequest user)
    {
        var res = await _userManager.CreateAsync(new User
        {
            UserName = user.Email,
            Email = user.Email,
            NormalizedEmail = user.Email,
            NormalizedUserName = user.Email,
        }, user.Password);

        if (!res.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Admin creation failed. Please check your info and try again.");
        }

        var created = await _userManager.FindByEmailAsync(user.Email);
        
        return Ok(created.Id);
    }
}