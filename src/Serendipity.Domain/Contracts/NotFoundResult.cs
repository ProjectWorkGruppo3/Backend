using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serendipity.Domain.Contracts;
public class NotFoundResult : ErrorResult
{
    public NotFoundResult(string message) : base(message)
    {
    }
}
