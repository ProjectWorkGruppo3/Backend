using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Services;

public interface IAlarmsService
{
    public Task<Result<IEnumerable<Alarm>>> GetLatest(string userEmail);
    public Task<IResult> SendNotifications(Alarm alarm);
}