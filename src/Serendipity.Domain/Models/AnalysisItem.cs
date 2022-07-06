using System.Text.Json.Serialization;
using Serendipity.Domain.Constants;

namespace Serendipity.Domain.Models;

public record AnalysisItem
(
    string Name,
    decimal Value,
    [property: JsonConverter(typeof(JsonStringEnumConverter))]
    Trends Trend
);
