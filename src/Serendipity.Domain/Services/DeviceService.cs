using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;

namespace Serendipity.Domain.Services;

public class DeviceService : IDeviceService
{
    private readonly IDeviceRepository _devices;
    private readonly IUserRepository _users;

    public DeviceService(IDeviceRepository devices, IUserRepository users)
    {
        _devices = devices;
        _users = users;
    }
    
    public async Task<IResult> GetUserDevices(string userId)
    {
        var user = await _users.FindUserById(userId);

        if (user is null)
        {
            return new ErrorResult($"No user by id {userId}.");
        }
        
        
        return await _devices.GetUserDevices(userId);
    }

    public async Task<IResult> RegisterDevice(string userId, Guid deviceId, string name)
    {
        var user = await _users.FindUserById(userId);

        if (user is null)
        {
            return new ErrorResult($"No user by id {userId}.");
        }
        
        
        return await _devices.RegisterDevice(userId, deviceId, name);
    }
    
    
}