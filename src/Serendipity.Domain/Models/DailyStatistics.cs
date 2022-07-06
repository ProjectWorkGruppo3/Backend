namespace Serendipity.Domain.Models;

public class DailyStatistics
{
    public DateTimeOffset Date { get; set; }

    public string GeolocalizationData { get; set; } = string.Empty; 

    public IEnumerable<AnalysisItem> Analysis { get; set; } =  Enumerable.Empty<AnalysisItem>();
}