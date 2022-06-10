namespace Serendipity.Domain.Contracts;

public class SuccessResult : IResult
{
    public SuccessResult()
    {
        Success = true;
    }

    public bool Success { get; init; }
}

public class SuccessResult<T> : Result<T>
{
    public SuccessResult(T data) : base(data)
    {
        Success = true;
    }
}