using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IAnalysisRepository
{
    public Task<IEnumerable<AnalysisItem>> GetLatestAnalysis();

    public Task<IResult> GetDailyStatistics();

    public Task<IEnumerable<AnalyticsChartData>> GetDataIngestedChartData();
    public Task<IEnumerable<AnalyticsChartData>> GetFallsChartData();
    
    public Task<IEnumerable<AnalyticsChartData>> GetSerendipityData();
}