using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Defaults;
using Serendipity.Infrastructure.Models;
using Serendipity.WebApi.Contracts.Requests;
using Serendipity.WebApi.Contracts.Responses;
using Serendipity.WebApi.Filters;

namespace Serendipity.WebApi.Controllers;

[Authorize(Roles = Roles.Admin)]
[Route("api/v1/[controller]")]
[ServiceFilter(typeof(InputValidationActionFilter))]
[ApiController]
public class AdminsController : Controller
{
    private readonly UserManager<User> _userManager;
    public AdminsController(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var admins = await _userManager.GetUsersInRoleAsync(Roles.Admin) ?? new List<User>();

        return Ok(admins.Select(a => new AdminUserResponse
        {
            Id = a.Id,
            Email = a.Email,
            Name = a.Name,
            Surname = a.Surname
        }));
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<IActionResult> Get(string userId)
    {
        var admin = await _userManager.FindByIdAsync(userId);

        if (admin is null) return NotFound();

        return Ok(new AdminUserResponse
        {
            Id = admin.Id,
            Email = admin.Email,
            Name = admin.Name,
            Surname = admin.Surname
        });
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] RegisterAdminRequest user)
    {
        var res = await _userManager.CreateAsync(new User
        {
            UserName = user.Email,
            Email = user.Email,
            NormalizedEmail = user.Email,
            NormalizedUserName = user.Email,
            Name = user.Name,
            Surname = user.Surname,
        }, user.Password);

        if (!res.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Admin creation failed. Please check your info and try again.");
        }

        var created = await _userManager.FindByEmailAsync(user.Email);

        await _userManager.AddToRoleAsync(created, Roles.Admin);
        
        return Ok(new AdminUserResponse
        {
            Id = created.Id,
            Email = created.Email,
            Name = created.Name,
            Surname = created.Surname
        });
    }

    [HttpPut]
    [Route("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, [FromBody] UpdateAdminRequest userUpdated)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) return NotFound();
        
        user.Name = userUpdated.Name;
        user.Surname = userUpdated.Surname;
        user.Email = userUpdated.Email; // FIXME
        user.UserName = userUpdated.Email; // FIXME
        user.NormalizedEmail = userUpdated.Email; // FIXME
        user.NormalizedUserName = userUpdated.Email; // FIXME
        

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Admin updating failed. Please check your info and try again.");
        }

        return NoContent();
    }
    [HttpDelete]
    [Route("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) return NotFound();

        var res = await _userManager.DeleteAsync(user);

        if (!res.Succeeded)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Admin deletion failed. Please check your info and try again.");
        }

        return NoContent();
    }
}