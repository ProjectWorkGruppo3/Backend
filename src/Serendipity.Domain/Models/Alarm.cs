namespace Serendipity.Domain.Models;

public abstract class Alarm
{
    public const string FallType = "FALL";
    public const string HeartBeatType = "HEARTBEAT";
    public const string BatteryType = "LOW_BATTERY";

    public string Type { get; set; } = null!;
    public DateTimeOffset Timestamp { get; set; }
    public string DeviceId { get; set; } = null!;
}