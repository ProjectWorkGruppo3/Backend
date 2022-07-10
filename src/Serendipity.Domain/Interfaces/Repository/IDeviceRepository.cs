using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IDeviceRepository
{
    public Task<int> GetTotalNumberDevices();
    public Task<IEnumerable<Device>> GetUserDevices(string userId);
    public Task<IResult> RegisterDevice(string userId, Guid deviceId, string name);

    public Task<Device?> GetDevice(Guid deviceId);
}