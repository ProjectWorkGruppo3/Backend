using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Services;

public interface IAlarmsService
{
    public Task<IResult> GetDeviceAlarms(string userId, Guid deviceId, int? start, int? limit);
    public Task<IResult> Insert(Alarm alarm);
}