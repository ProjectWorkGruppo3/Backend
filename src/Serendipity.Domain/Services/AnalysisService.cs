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
}