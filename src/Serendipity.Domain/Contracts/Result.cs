namespace Serendipity.Domain.Contracts;

public interface IResult
{
    public bool Success { get; init; }
}

public abstract class Result<T> : IResult
{
    private T? _data;

    protected Result(T? data)
    {
        Data = data;
    }

    public T? Data
    {
        get => _data ?? throw new Exception($"You can't access .{nameof(Data)} when .{nameof(Success)} is false");
        set => _data = value;
    }

    public bool Success { get; init; }
}