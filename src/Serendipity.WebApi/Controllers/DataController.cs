using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;
using Serendipity.WebApi.Contracts;
using Serendipity.WebApi.Contracts.Requests;
using Serendipity.WebApi.Filters;

namespace Serendipity.WebApi.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
[ServiceFilter(typeof(InputValidationActionFilter))]
public class DataController : Controller
{
    private readonly IDeviceDataService _service;

    public DataController(IDeviceDataService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] InsertDeviceDataRequest request)
    {
        var result = await _service.Insert(new DeviceDataModel
        {
            Id = request.DeviceId,
            Data = request.Data
        });

        return result switch
        {
            SuccessResult => Ok(),
            ErrorResult => StatusCode(500),
            _ => StatusCode(500)
        };
    }
}