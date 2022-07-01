using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class DeviceDataService : IDeviceDataService
{
    private readonly IDeviceDataRepository _repo;

    public DeviceDataService(IDeviceDataRepository repo)
    {
        _repo = repo;
    }

    public async Task<IResult> Insert(DeviceDataModel data)
    {
        data.Timestamp = DateTimeOffset.Now;
        return await _repo.Insert(data);
    }
}