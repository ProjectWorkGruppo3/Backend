namespace Serendipity.Infrastructure.Models;

public abstract class Alarm
{
    public string Type { get; set; } = null!;
    public DateTimeOffset Timestamp { get; set; }
    public string DeviceId { get; set; } = null!;
}