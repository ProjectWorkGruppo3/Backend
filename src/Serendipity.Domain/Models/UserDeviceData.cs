using Serendipity.Domain.Constants;

namespace Serendipity.Domain.Models;

public class UserDeviceData
{
    public DateTime LastUpdate { get; set; }
    public Location LastLocation { get; set; }
    public States LastState { get; set; }
    public int TotalAlarms { get; set; }
    public IEnumerable<AnalysisItem> Analysis { get; set; } =  Enumerable.Empty<AnalysisItem>();
}


public class Location
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}