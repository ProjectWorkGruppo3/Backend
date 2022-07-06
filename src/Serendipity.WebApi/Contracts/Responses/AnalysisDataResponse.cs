using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Serendipity.WebApi.Contracts.Responses;

public enum AnalysisTrending
{
    [EnumMember(Value = nameof(Up))]
    Up,
    [EnumMember(Value = nameof(Down))]
    Down,
    [EnumMember(Value = nameof(Equal))]
    Equal
}

public class AnalysisDataResponse
{
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AnalysisTrending Trending { get; set; }
}