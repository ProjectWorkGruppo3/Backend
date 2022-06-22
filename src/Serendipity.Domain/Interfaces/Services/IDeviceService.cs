using Serendipity.Domain.Contracts;

namespace Serendipity.Domain.Interfaces.Services;

public interface IDeviceService
{
    public Task<IResult> RegisterDevice(string userId, Guid deviceId, string name);
}