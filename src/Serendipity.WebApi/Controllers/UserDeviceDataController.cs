using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;
using NotFoundResult = Serendipity.Domain.Contracts.NotFoundResult;
using User = Serendipity.Infrastructure.Models.User;

namespace Serendipity.WebApi.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserDeviceDataController : ControllerBase
    {
        private readonly IDeviceDataService _deviceDataService;
        private readonly UserManager<User> _userManager;
        

        public UserDeviceDataController(IDeviceDataService deviceDataService, UserManager<User> userManager)
        {
            _deviceDataService = deviceDataService;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("{deviceId}")]
        public async Task<ActionResult> GetUserDeviceData(Guid deviceId)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null) return Unauthorized();
            var userDeviceData = await _deviceDataService.GetUserDeviceData(user.Id, deviceId);

            return userDeviceData switch
            {
                SuccessResult<UserDeviceData> successResult => Ok(successResult.Data),
                NotFoundResult => NotFound(),
                ErrorResult errorResult => StatusCode(500, errorResult.Message),
                _ => StatusCode(500)
            };
        }

        [HttpGet]
        [Route("{deviceId}/{statisticName}")]
        public async Task<ActionResult> GetStatisticsUserDeviceData(Guid deviceId, string statisticName)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user is null) return Unauthorized();

            var data = await _deviceDataService.GetUserDeviceStatisticData(user.Id, deviceId, statisticName);

            return data switch
            {
                SuccessResult<IEnumerable<AnalyticsChartData>> successResult => Ok(successResult.Data),
                NotFoundResult => NotFound(),
                ErrorResult errorResult => StatusCode(500, errorResult.Message),
                _ => StatusCode(500)
            };
        }
    }
}
