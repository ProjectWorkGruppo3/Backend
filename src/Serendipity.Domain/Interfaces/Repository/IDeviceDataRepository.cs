using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IDeviceDataRepository
{
    public Task<IResult> Insert(DeviceDataModel data);

    public Task<IEnumerable<DeviceDataModel>> GetLatestDeviceData(string deviceId);
}