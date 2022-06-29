using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Services;

public interface IReportService
{
    public Task<IResult> GetReports();

    public Task<IResult> DownloadFile(string filename);
}