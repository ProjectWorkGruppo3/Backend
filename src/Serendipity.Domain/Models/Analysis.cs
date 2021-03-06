namespace Serendipity.Domain.Models;

public class Analysis
{
    public int AdminsCount { get; set; }
    public int DevicesCount { get; set; }
    public IEnumerable<AnalysisItem> LastAnalysis { get; set; } =  Enumerable.Empty<AnalysisItem>();
    public IEnumerable<Report> LatestReports { get; set; } = Enumerable.Empty<Report>();
}
