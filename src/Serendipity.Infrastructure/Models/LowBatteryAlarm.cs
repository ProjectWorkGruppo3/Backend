namespace Serendipity.Infrastructure.Models;

public class LowBatteryAlarm : Alarm
{
    public int BatteryCharge { get; set; }
}