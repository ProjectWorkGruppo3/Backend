namespace Serendipity.Domain.Models;

public abstract class Alarm
{
    public DateTimeOffset Timestamp { get; set; }
    public string UserId { get; set; } = null!;
    
}