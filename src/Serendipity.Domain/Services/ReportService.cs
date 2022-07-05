using Serendipity.Domain.Contracts;
using Serendipity.Domain.Interfaces.Repository;
using Serendipity.Domain.Interfaces.Services;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Services;

public class ReportService : IReportService
{
    
    private readonly IReportRepository _repo;

    public ReportService(IReportRepository repo)
    {
        _repo = repo;
    }

    public Task<IResult> GetReports()
    {
        return _repo.GetReports()
            .ContinueWith<IResult>(res =>
            {
                if (res.Exception != null)
                {
                    return new ErrorResult("");
                }
                return new SuccessResult<IEnumerable<Report>>(res.Result);
            });
    }

    public async Task<IResult> DownloadFile(string filename)
    {
        return await _repo.DownloadFile(filename);        
    }
}