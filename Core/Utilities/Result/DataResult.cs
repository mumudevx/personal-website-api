namespace Core.Utilities.Result;

public class DataResult<T> : Result, IDataResult<T>
{
    protected DataResult(T data, bool success, string message) : base(success, message)
    {
        Data = data;
    }

    protected DataResult(T data, bool success) : base(success)
    {
        Data = data;
    }

    public T Data { get; }
}