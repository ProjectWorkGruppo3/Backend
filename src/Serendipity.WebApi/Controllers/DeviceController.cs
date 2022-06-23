using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Infrastructure.Models;
using Serendipity.WebApi.Contracts;
using Serendipity.WebApi.Contracts.Requests;
using Serendipity.WebApi.Contracts.Responses;
using Serendipity.WebApi.Filters;

namespace Serendipity.WebApi.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ServiceFilter(typeof(InputValidationActionFilter))]
[ApiController]
public class DeviceController : Controller
{
    private readonly IDeviceService _deviceService;
    private readonly UserManager<User> _userManager;

    public DeviceController(IDeviceService deviceService, UserManager<User> userManager)
    {
        _deviceService = deviceService;
        _userManager = userManager;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserDevices() 
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null) return Unauthorized();

        var res = await _deviceService.GetUserDevices(user.Id);

        return res switch
        {
            SuccessResult<IEnumerable<Domain.Models.Device>> successResult => Ok(successResult.Data!.Select(e =>
                new DeviceResponse
                {
                    Name = e.Name,
                    DeviceId = e.Id
                })),
            ErrorResult errorResult => StatusCode(500, errorResult.Message),
            _ => StatusCode(500)
        };
    }
    [HttpPost]
    public async Task<IActionResult> AddDeviceToUser([FromBody] RegisterDeviceRequest registerDeviceRequest)
    {
        
        var user = await _userManager.GetUserAsync(User);

        if (user is null) return Unauthorized();
        
        var res = await _deviceService.RegisterDevice(user.Id, registerDeviceRequest.Id!.Value, registerDeviceRequest.Name!);

        return res switch
        {
            SuccessResult<Domain.Models.Device> => Ok(),
            ErrorResult errorResult => StatusCode(500, errorResult.Message),
            _ => StatusCode(500)
        };
    }
}