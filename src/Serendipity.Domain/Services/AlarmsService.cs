using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class AlarmsService : IAlarmsService
{
    private readonly IAlarmsRepository _alarmsRepository;
    private IAlarmsService _alarmsServiceImplementation;

    public AlarmsService(IAlarmsRepository alarmsRepository)
    {
        _alarmsRepository = alarmsRepository;
    }

    public async Task<Result<IEnumerable<Alarm>>> GetLatest(string userEmail)
    {
        return await _alarmsRepository.GetLatest(userEmail);
    }

    public Task<IResult> SendNotifications(Alarm alarm)
    {
        throw new NotImplementedException();
    }
}