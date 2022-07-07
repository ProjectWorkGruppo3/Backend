using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Services;

public interface IDeviceDataService
{
    public Task<IResult> Insert(DeviceDataModel data);

    public Task<IResult> GetUserDeviceData(string userId, Guid deviceId);

    public Task<IResult> GetUserDeviceStatisticData(string userId, Guid deviceId, string statisticName);
}