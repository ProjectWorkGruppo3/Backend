using System.Text.Json.Serialization;
using Serendipity.Domain.Constants;

namespace Serendipity.Domain.Models;

public class UserDeviceData
{
    public Guid DeviceId { get; set; }
    public DateTimeOffset LastUpdate { get; set; }
    public Location LastLocation { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public States LastState { get; set; }
    public int TotalAlarms { get; set; }
    public IEnumerable<AnalysisItem> Analysis { get; set; } =  Enumerable.Empty<AnalysisItem>();
    
    public int Battery { get; set; }
    
}


public class Location
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}