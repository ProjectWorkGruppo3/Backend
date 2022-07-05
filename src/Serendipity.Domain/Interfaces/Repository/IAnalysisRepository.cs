using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;

public interface IAnalysisRepository
{
    public Task<IEnumerable<AnalysisItem>> GetLatestAnalysis();
}