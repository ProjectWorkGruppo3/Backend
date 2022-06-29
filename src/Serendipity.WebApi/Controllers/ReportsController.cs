using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Defaults;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;
using User = Serendipity.Infrastructure.Models.User;

namespace Serendipity.WebApi.Controllers;

[Authorize]
[Route("api/v1/[controller]")]

public class ReportsController : Controller
{
    private readonly IReportService _reportService;
    private readonly UserManager<User> _userManager;

    public ReportsController(IReportService reportService, UserManager<User> userManager)
    {
        _reportService = reportService;
        _userManager = userManager;
    }

    [HttpGet]
    
    public async Task<IActionResult> GetReports()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null) return Unauthorized();

        // if (!User.IsInRole(Roles.Admin)) return Unauthorized();
        
        
        var reports = await _reportService.GetReports();

        return reports switch
        {
            SuccessResult<IEnumerable<Report>> successResult => Ok(successResult.Data),
            ErrorResult errorResult => StatusCode(500, errorResult.Message),
            _ => StatusCode(500)
        };
    }
}