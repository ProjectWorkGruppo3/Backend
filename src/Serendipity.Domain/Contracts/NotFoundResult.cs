namespace Serendipity.Domain.Contracts;
public class NotFoundResult : ErrorResult
{
    public NotFoundResult(string message) : base(message)
    {
    }
}
