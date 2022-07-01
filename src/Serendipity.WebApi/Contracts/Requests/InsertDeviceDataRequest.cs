using Serendipity.Domain.Models;

namespace Serendipity.WebApi.Contracts.Requests;

public class InsertDeviceDataRequest
{
    public Guid DeviceId { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public DeviceData Data { get; set; } = null!;
}