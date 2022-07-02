using Serendipity.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serendipity.Domain.Models;

namespace Serendipity.Domain.Interfaces.Repository;
public interface IReportRepository
{
    public Task<IEnumerable<Report>> GetReports();

    public Task<IResult> DownloadFile(string filename);
}
