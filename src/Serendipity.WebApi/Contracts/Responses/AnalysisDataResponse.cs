using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Serendipity.WebApi.Contracts.Responses;

public enum AnalysisTrending
{
    [EnumMember(Value = nameof(UP))]
    UP,
    [EnumMember(Value = nameof(DOWN))]
    DOWN,
    [EnumMember(Value = nameof(EQUAL))]
    EQUAL
}

public class AnalysisDataResponse
{
    public string Name { get; set; }
    public decimal Value { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public AnalysisTrending Trending { get; set; }
}