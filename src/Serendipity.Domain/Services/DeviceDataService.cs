using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class DeviceDataService : IDeviceDataService
{
    private readonly IDeviceDataRepository _repo;
    private readonly IDeviceRepository _deviceRepository;
    public DeviceDataService(IDeviceDataRepository repo, IDeviceRepository deviceRepository)
    {
        _repo = repo;
        _deviceRepository = deviceRepository;
    }

    public async Task<IResult> Insert(DeviceDataModel data)
    {
        data.Timestamp = DateTimeOffset.Now;
        return await _repo.Insert(data);
    }

    public async Task<IResult> GetUserDeviceData(string userId, Guid deviceId)
    {
        // TODO check if device exist
        // TODO check if the user registered the device
        var userDevicesResult = await _deviceRepository.GetUserDevices(userId);

        var isUserDevice = userDevicesResult.Select(el => el.Id).Contains(deviceId);

        if (!isUserDevice)
        {
            return new NotFoundResult("Device not found");
        }
        
        
        // TODO return UserDeviceData
        
        /*return new SuccessResult<UserDeviceData>(new UserDeviceData
        {
            Analysis = 
        })*/
        throw new NotImplementedException();
    }
}