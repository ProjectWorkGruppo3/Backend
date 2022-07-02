using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;

namespace Serendipity.Domain.Services;

public class AnalysisService : IAnalysisService
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly IUserRepository _userRepository;
    private readonly IReportRepository _reportRepository;

    public AnalysisService(IDeviceRepository deviceRepository, IUserRepository userRepository, IReportRepository reportRepository)
    {
        _deviceRepository = deviceRepository;
        _userRepository = userRepository;
        _reportRepository = reportRepository;
    }

    public async Task<IResult> GetGeneralStatistics()
    {
        try
        {
            var totalAdmins = await _userRepository.GetNumberOfAdmins();
            var totalDevices = await _deviceRepository.GetTotalNumberDevices();
            var lastReports = (await _reportRepository.GetReports()).Take(5);


            return new SuccessResult<object>(
                new
                {
                    TotalAdmins = totalAdmins,
                    TotalBracelets = totalDevices,
                    LastReports = lastReports
                });
        }
        catch (Exception e)
        {
            return new ErrorResult("Error");
        }
    }
}