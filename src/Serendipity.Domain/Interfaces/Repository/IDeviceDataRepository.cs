using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IDeviceDataRepository
{
    public Task<IResult> Insert(DeviceDataModel data);

    public Task<IEnumerable<DeviceDataModel>> GetLatestDeviceData(string deviceId);
    public Task<IEnumerable<AnalyticsChartData>> GetSerendipityChartData(string deviceId);

    public Task<IEnumerable<AnalyticsChartData>> GetFallsChartData(string deviceId);
    
    public Task<IEnumerable<AnalyticsChartData>> GetHeartbeatChartData(string deviceId);
    public Task<IEnumerable<AnalyticsChartData>> GetStandingsChartData(string deviceId);
    
    public Task<IEnumerable<AnalyticsChartData>> GetStepsChartData(string deviceId);
}