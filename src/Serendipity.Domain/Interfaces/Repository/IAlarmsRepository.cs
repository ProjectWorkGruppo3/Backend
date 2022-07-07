using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IAlarmsRepository
{
    public Task<IEnumerable<Alarm>> GetDeviceAlarms(Guid deviceId, int start, int limit);
    public Task Insert(Alarm alarm);
}