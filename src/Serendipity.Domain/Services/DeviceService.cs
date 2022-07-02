using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

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

    public async Task<IResult> GetTotalNumberDevices()
    {
        try
        {
            var totalNumber = await _devices.GetTotalNumberDevices();

            return new SuccessResult<int>(totalNumber);
        }
        catch (Exception e)
        {
            return new ErrorResult("Something went wrong when try to get total number devices");
        }
    }

    public async Task<IResult> GetUserDevices(string userId)
    {
        var user = await _users.FindUserById(userId);

        var userR = user switch
        {
            SuccessResult<Device> successResult => successResult.Data!,
            _ => null
        };
        
        if (userR is null)
        {
            return new ErrorResult($"No user by id {userId}.");
        }
        
        
        return await _devices.GetUserDevices(userId);
    }

    public async Task<IResult> RegisterDevice(string userId, Guid deviceId, string name)
    {
        var user = await _users.FindUserById(userId);

        return user switch
        {
            SuccessResult<User> => await _devices.RegisterDevice(userId, deviceId, name),
            NotFoundResult notFoundResult => notFoundResult,
            { } e => e
        };
    }
    
    
}