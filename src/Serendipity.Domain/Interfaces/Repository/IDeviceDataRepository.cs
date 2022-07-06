using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IDeviceDataRepository
{
    public Task<IResult> Insert(DeviceDataModel data);

    public Task<DeviceDataModel> GetLatestDeviceData(string deviceId);
}