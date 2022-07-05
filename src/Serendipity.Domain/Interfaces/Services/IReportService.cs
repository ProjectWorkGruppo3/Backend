using Serendipity.Domain.Contracts;

namespace Serendipity.Domain.Interfaces.Services;

public interface IReportService
{
    public Task<IResult> GetReports();

    public Task<IResult> DownloadFile(string filename);
}