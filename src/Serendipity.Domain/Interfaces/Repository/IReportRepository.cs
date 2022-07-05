using Serendipity.Domain.Contracts;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;
public interface IReportRepository
{
    public Task<IEnumerable<Report>> GetReports();
    public Task<IEnumerable<Report>> GetLatestReports(int count);

    public Task<IResult> DownloadFile(string filename);
}
