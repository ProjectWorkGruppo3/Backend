using Serendipity.Domain.Models;

namespace Serendipity.WebApi.Contracts.Responses;

public class GeneralStatisticsResponse
{
    public int TotalAdmins { get; set; }
    public int TotalBracelets { get; set; }
    public IEnumerable<Report> LastReports { get; set; }
    public IEnumerable<AnalysisDataResponse> Analysis { get; set; }
}