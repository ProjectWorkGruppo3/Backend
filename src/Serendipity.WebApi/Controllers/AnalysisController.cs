using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Defaults;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;
using NotFoundResult = Serendipity.Domain.Contracts.NotFoundResult;

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
            SuccessResult<Analysis> successResult => Ok(successResult.Data),
            ErrorResult err => StatusCode(StatusCodes.Status500InternalServerError, err.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }


    [HttpGet]
    [Route("daily")]
    public async Task<IActionResult> GetAnalyticsData()
    {
        var result = await _analysisService.GetDailyStatistics();

        return result switch
        {
            SuccessResult<DailyStatistics> successResult => Ok(successResult.Data),
            ErrorResult err => StatusCode(StatusCodes.Status500InternalServerError, err.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
    
    [HttpGet]
    [Route("{statisticName}/data")]
    public async Task<IActionResult> GetChartData(string statisticName)
    {
        var result = await _analysisService.GetAnalysisChartData(statisticName);
        
        return result switch
        {
            SuccessResult<IEnumerable<AnalyticsChartData>> successResult => Ok(successResult.Data),
            NotFoundResult notFoundResult => NotFound(notFoundResult.Message),
            ErrorResult err => StatusCode(StatusCodes.Status500InternalServerError, err.Message),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
