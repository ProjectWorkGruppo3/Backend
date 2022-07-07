using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;
using Serendipity.WebApi.Contracts.Responses;
using Serendipity.WebApi.Filters;
using Serendipity.WebApi.ModelBinders;
using User = Serendipity.Infrastructure.Models.User;

namespace Serendipity.WebApi.Controllers;


[Authorize]
[Route("api/v1/[controller]")]
[ServiceFilter(typeof(InputValidationActionFilter))]
[ApiController]
public class AlarmsController : Controller
{
    private readonly IAlarmsService _alarms;
    private readonly UserManager<User> _userManager;
    public AlarmsController(IAlarmsService alarms, UserManager<User> userManager)
    {
        _alarms = alarms;
        _userManager = userManager;
    }
    
    [HttpGet]
    [Route("{deviceId}")]
    public async Task<IActionResult> Latest(Guid deviceId,int? start, int? limit)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null) return Unauthorized();
        
        var latest = await _alarms.GetDeviceAlarms(user.Id, deviceId, start, limit);

        return latest switch
        {
            SuccessResult<IEnumerable<Alarm>> successResult => Ok(successResult.Data!.Select(el => new AlarmResponse
            {
                Date = el.Timestamp,
                Type = el.Type
            })),
            ErrorResult<IEnumerable<Alarm>> errorResult => StatusCode(StatusCodes.Status500InternalServerError, new { errorResult.Message, errorResult.Errors}),
            _ => new StatusCodeResult(500)
        };
    }

    [HttpPost]
    public async Task<IActionResult> Insert([ModelBinder(BinderType = typeof(AlarmModelBinder))] Alarm alarm)
    {
        var res = await _alarms.Insert(alarm);
        return res switch
        {
            SuccessResult => Ok(),
            ErrorResult err => StatusCode(StatusCodes.Status500InternalServerError, err.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}

