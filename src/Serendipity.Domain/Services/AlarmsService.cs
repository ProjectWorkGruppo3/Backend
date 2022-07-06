using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class AlarmsService : IAlarmsService
{
    private readonly IAlarmsRepository _alarmsRepository;

    public AlarmsService(IAlarmsRepository alarmsRepository)
    {
        _alarmsRepository = alarmsRepository;
    }

    public async Task<Result<IEnumerable<Alarm>>> GetLatest(string userEmail)
    {
        return await _alarmsRepository.GetLatest(userEmail);
    }

    public async Task<IResult> Insert(Alarm alarm)
    {
        // TODO: send email

        return await _alarmsRepository.Insert(alarm);
    }
}