namespace Serendipity.Domain.Models;

public class LowBatteryAlarm : Alarm
{
    public int BatteryCharge { get; set; }
}