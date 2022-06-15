using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IAlarmsRepository
{
    public Task<Result<IEnumerable<Alarm>>> GetLatest(string userEmail);
}