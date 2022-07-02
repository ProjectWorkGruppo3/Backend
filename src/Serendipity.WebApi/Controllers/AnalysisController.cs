using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Defaults;
using Serendipity.Domain.Interfaces.Services;

namespace Serendipity.WebApi.Controllers;

[Authorize(Roles = Roles.Admin)]
[Route("api/v1/[controller]")]
[ApiController]
public class AnalysisController : ControllerBase
{
    private readonly IAnalysisService _analysisService;

    public AnalysisController(IAnalysisService analysisService)
    {
        _analysisService = analysisService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGeneralStatistics()
    {
        var result = await _analysisService.GetGeneralStatistics();

        return result switch
        {
            SuccessResult<object> successResult => Ok(successResult.Data),
            ErrorResult err => StatusCode(StatusCodes.Status500InternalServerError, err.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
