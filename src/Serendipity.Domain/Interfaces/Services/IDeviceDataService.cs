using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Services;

public interface IDeviceDataService
{
    public Task<IResult> Insert(DeviceDataModel data);
}