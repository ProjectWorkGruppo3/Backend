using System.Runtime.Serialization;
using System.Text.Json.Serialization;


namespace Serendipity.Domain.Models;

public class Analysis
{
    public int AdminsCount { get; set; }
    public int DevicesCount { get; set; }
    public IEnumerable<AnalysisItem> LastAnalysis { get; set; } =  Enumerable.Empty<AnalysisItem>();
    public IEnumerable<Report> LatestReports { get; set; } = Enumerable.Empty<Report>();
}

public record AnalysisItem
(
    string Name,
    decimal Value,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Trends Trend
);

public enum Trends
{
    [EnumMember(Value = nameof(Up))]
    Up,
    [EnumMember(Value = nameof(Down))]
    Down,
    [EnumMember(Value = nameof(Equal))]
    Equal
}