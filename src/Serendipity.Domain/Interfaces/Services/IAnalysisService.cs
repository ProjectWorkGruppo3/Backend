using Serendipity.Domain.Contracts;

namespace Serendipity.Domain.Interfaces.Services;

public interface IAnalysisService
{
    public Task<IResult> GetGeneralStatistics();
    
    public Task<IResult> GetDailyStatistics();
}