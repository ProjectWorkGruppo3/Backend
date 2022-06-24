namespace Serendipity.Domain.Contracts;

public class ErrorResult : IErrorResult
{
    public ErrorResult(string message) : this(message, Array.Empty<Error>())
    {
    }

    public ErrorResult(string message, IReadOnlyCollection<Error>? errors = default)
    {
        Message = message;
        Success = false;
        Errors = errors ?? Array.Empty<Error>();
    }

    public string Message { get; }
    public IReadOnlyCollection<Error> Errors { get; }
    public bool Success { get; init; }
}

public class ErrorResult<T> : Result<T>, IErrorResult
{
    public ErrorResult(string message) : this(message, Array.Empty<Error>())
    {    
    }

    public ErrorResult(string message, IReadOnlyCollection<Error>? errors = default) : base(default)
    {
        Message = message;
        Success = false;
        Errors = errors ?? Array.Empty<Error>();
    }

    public string Message { get; set; }
    public IReadOnlyCollection<Error> Errors { get; }
}



internal interface IErrorResult : IResult
{
    string Message { get; }
    IReadOnlyCollection<Error> Errors { get; }
}