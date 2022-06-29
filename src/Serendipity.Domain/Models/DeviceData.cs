namespace Serendipity.Domain.Models;

public class DeviceDataModel
{
    public Guid Id { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public DeviceData Data { get; set; } = null!;
}

public class DeviceData
{
    public int Serendipity { get; set; }
    public int StepsWalked { get; set; }
    public int Heartbeat { get; set; }
    public int NumberOfFalls { get; set; }
    public int Battery { get; set; }
    public int Standings { get; set; }
}