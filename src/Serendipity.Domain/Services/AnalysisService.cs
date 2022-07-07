using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class AnalysisService : IAnalysisService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReportRepository _reportRepository;
    private readonly IAnalysisRepository _analysisRepository;

    public AnalysisService(IDeviceRepository deviceRepository, IUserRepository userRepository, IReportRepository reportRepository, IAnalysisRepository analysisRepository)
    {
        _deviceRepository = deviceRepository;
        _userRepository = userRepository;
        _reportRepository = reportRepository;
        _analysisRepository = analysisRepository;
    }

    public async Task<IResult> GetGeneralStatistics()
    {
        try
        {
            var totalAdmins = await _userRepository.GetNumberOfAdmins();
            var totalDevices = await _deviceRepository.GetTotalNumberDevices();
            var lastReports = await _reportRepository.GetLatestReports(5);
            var lastAnalysis = await _analysisRepository.GetLatestAnalysis();


            return new SuccessResult<Analysis>(
                new Analysis()
                {
                    AdminsCount = totalAdmins,
                    DevicesCount = totalDevices,
                    LatestReports = lastReports,
                    LastAnalysis = lastAnalysis
                });
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message + e.InnerException);
        }
    }

    public Task<IResult> GetDailyStatistics()
    {
        return _analysisRepository.GetDailyStatistics();
    }

    public async Task<IResult> GetAnalysisChartData(string statisticName)
    {
        try
        {
            var analysis = await _analysisRepository.GetLatestAnalysis();

            var statisticExist = analysis.Select(e => e.Name.ToLower()).Contains(statisticName.ToLower());

            if (!statisticExist)
            {
                return new NotFoundResult("Statistic not found");
            }

            var fetchFunction = GetFetchFunc(statisticName);
            
            var chartData = await fetchFunction();
            
            return new SuccessResult<IEnumerable<AnalyticsChartData>>(chartData);
        }
        catch (Exception e)
        {
            return new ErrorResult(e.Message);
        }
    }

    private Func<Task<IEnumerable<AnalyticsChartData>>> GetFetchFunc(string statisticName)
    {
        switch (statisticName.ToLower())
        {
            case "falls":
                return () => _analysisRepository.GetDataIngestedChartData();
            case "dataingested":
                return () => _analysisRepository.GetDataIngestedChartData();
            case "serendipity":
                return () => _analysisRepository.GetSerendipityData();
            default:
                throw new Exception("Statistic not found");
        }
    }
    
    
}