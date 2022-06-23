using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;
using Serendipity.WebApi.Filters;

namespace Serendipity.WebApi.Controllers;


[Authorize]
[Route("api/v1/[controller]")]
[ServiceFilter(typeof(InputValidationActionFilter))]
[ApiController]
public class AlarmsController : Controller
{
    private readonly IAlarmsService _alarms;
    public AlarmsController(IAlarmsService alarms)
    {
        _alarms = alarms;
    }
    
    [HttpGet]
    public async Task<IActionResult> Latest()
    {
        var email = User.Claims.First(c => c.Type is ClaimTypes.Email).Value;
        var latest = await _alarms.GetLatest(email);

        return latest switch
        {
            SuccessResult<IEnumerable<Alarm>> successResult => Ok(successResult.Data),
            ErrorResult<IEnumerable<Alarm>> errorResult => StatusCode(StatusCodes.Status500InternalServerError, new { errorResult.Message, errorResult.Errors}),
            _ => new StatusCodeResult(500)
        };
    }
}

