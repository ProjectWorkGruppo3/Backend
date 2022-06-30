namespace Serendipity.Infrastructure.Models;

public class GlobalStatistics
{
    public DateTimeOffset Giorno { get; set; }
    public long DataIngested { get; set; }
    public long Falls { get; set; }
    public int Serendipity { get; set; }
    public string LocationDensity { get; set; }
}