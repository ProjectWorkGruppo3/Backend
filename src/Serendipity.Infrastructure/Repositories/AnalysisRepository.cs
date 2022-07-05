using Microsoft.EntityFrameworkCore;
using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Models;
using Serendipity.Infrastructure.Database;

namespace Serendipity.Infrastructure.Repositories;

public class AnalysisRepository : IAnalysisRepository
{
    private readonly AppDbContext _db;

    public AnalysisRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<AnalysisItem>> GetLatestAnalysis()
    {
        try
        {
            var stats = await _db.GlobalStatistics.OrderByDescending(s => s.Date).FirstAsync();
            var old = await _db.GlobalStatistics.OrderByDescending(s => s.Date).Skip(1).FirstOrDefaultAsync();

            List<AnalysisItem> list = new()
            {
                new AnalysisItem
                (
                    Name: nameof(stats.Serendipity),
                    Value: stats.Serendipity,
                    Trend: GetTrend(old?.Serendipity, stats.Serendipity)
                ),
                new AnalysisItem
                (
                    Name: nameof(stats.Falls),
                    Value: stats.Falls,
                    Trend: GetTrend(old?.Falls, stats.Falls)
                ),
                new AnalysisItem
                (
                    nameof(stats.DataIngested),
                    stats.DataIngested,
                    GetTrend(old?.DataIngested, stats.DataIngested)
                )
            };

            return list;
        }
        catch (Exception)
        {
            return Enumerable.Empty<AnalysisItem>();
        }
    }

    public async Task<IResult> GetDailyStatistics()
    {
        try
        {
            var latestAnalysis = await GetLatestAnalysis();
            var stats = await _db.GlobalStatistics.OrderByDescending(s => s.Date).FirstAsync();

            return new SuccessResult<DailyStatistics>(new  DailyStatistics
            {
                Analysis = latestAnalysis,
                Date = stats.Date,
                GeolocalizationData = stats.LocationDensity
            });
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
    }

    private Trends GetTrend(decimal? oldValue, decimal newValue) 
    {
        if(oldValue is null || oldValue == newValue) 
        {
            return Trends.Equal;
        }
        
        return oldValue > newValue ? Trends.Down : Trends.Up;
    }
}