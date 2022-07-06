using Serendipity.Domain.Constants;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class DeviceDataService : IDeviceDataService
{
    private readonly IDeviceDataRepository _repo;
    private readonly IDeviceRepository _deviceRepository;
    private readonly IDeviceDataRepository _deviceDataRepository;
    public DeviceDataService(IDeviceDataRepository repo, IDeviceRepository deviceRepository, IDeviceDataRepository deviceDataRepository)
    {
        _repo = repo;
        _deviceRepository = deviceRepository;
        _deviceDataRepository = deviceDataRepository;
    }

    public async Task<IResult> Insert(DeviceDataModel data)
    {
        data.Timestamp = DateTimeOffset.Now;
        return await _repo.Insert(data);
    }

    public async Task<IResult> GetUserDeviceData(string userId, Guid deviceId)
    {
        var userDevicesResult = await _deviceRepository.GetUserDevices(userId);

        var isUserDevice = userDevicesResult.Select(el => el.Id).Contains(deviceId);

        if (!isUserDevice)
        {
            return new NotFoundResult("Device not found");
        }

        var latestDeviceData = await _deviceDataRepository.GetLatestDeviceData(deviceId.ToString());

        var deviceDataModels = latestDeviceData.ToList();
        var newMeasurement = deviceDataModels.First();
        var oldMeasurement = deviceDataModels.Skip(1).First();

        return new SuccessResult<UserDeviceData>(new UserDeviceData
        {
            DeviceId = deviceId,
            Analysis = new List<AnalysisItem>
            {
                new(
                    Name: nameof(newMeasurement.Data.Serendipity),
                    Value: newMeasurement.Data.Serendipity,
                    Trend: GetTrend(oldMeasurement?.Data.Serendipity, newMeasurement.Data.Serendipity)
                ),
                new(
                    Name: nameof(newMeasurement.Data.NumberOfFalls),
                    Value: newMeasurement.Data.NumberOfFalls,
                    Trend: GetTrend(oldMeasurement?.Data.NumberOfFalls, newMeasurement.Data.NumberOfFalls)
                ),
                new(
                    Name: nameof(newMeasurement.Data.Heartbeat),
                    Value: newMeasurement.Data.Heartbeat,
                    Trend: GetTrend(oldMeasurement?.Data.Heartbeat, newMeasurement.Data.Heartbeat)
                ),
                new(
                    Name: nameof(newMeasurement.Data.Standings),
                    Value: newMeasurement.Data.Standings,
                    Trend: GetTrend(oldMeasurement?.Data.Standings, newMeasurement.Data.Standings)
                ),
                new(
                    Name: nameof(newMeasurement.Data.StepsWalked),
                    Value: newMeasurement.Data.StepsWalked,
                    Trend: GetTrend(oldMeasurement?.Data.StepsWalked, newMeasurement.Data.StepsWalked)
                ),
            },
            LastLocation = new Location
            {
                Latitude = newMeasurement.Data.Latitude,
                Longitude = newMeasurement.Data.Longitude
            },
            LastState = newMeasurement.Data.State,
            LastUpdate = newMeasurement.Timestamp,
            Battery = newMeasurement.Data.Battery,
            TotalAlarms = 0 // FIXME
        });
    }
    
    private Trends GetTrend(decimal? oldValue, decimal newValue) 
    {
        if(oldValue is null || oldValue == newValue) 
        {
            return Trends.Equal;
        }
        
        return oldValue > newValue ? Trends.Down : Trends.Up;
    }
}