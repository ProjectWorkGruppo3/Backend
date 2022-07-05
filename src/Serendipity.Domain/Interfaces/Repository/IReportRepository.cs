using Serendipity.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serendipity.Domain.Interfaces.Repository;
public interface IReportRepository
{
    public Task<IResult> GetReports();

    public Task<IResult> DownloadFile(string filename);
}
