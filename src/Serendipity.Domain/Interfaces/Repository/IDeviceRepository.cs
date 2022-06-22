using Serendipity.Domain.Contracts;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IDeviceRepository
{
    public Task<IResult> GetUserDevices(string userId);
    public Task<IResult> RegisterDevice(string userId, Guid deviceId, string name);
}