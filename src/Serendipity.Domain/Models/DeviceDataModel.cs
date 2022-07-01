namespace Serendipity.Domain.Models;

public class DeviceDataModel
{
    public Guid Uuid { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public DeviceData Data { get; set; } = null!;
}