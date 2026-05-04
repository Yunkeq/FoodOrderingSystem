namespace FoodOrderingSystem.Application.Common.ResultPattern;

public class Result
{
    protected Result(Error error)
    {
        Error = error;
        IsSuccess = error.ErrorCode == ErrorCode.None;
    }

    public bool IsSuccess { get; }
    public Error Error { get; }

    public static Result Success() => new Result(Error.None);
    public static Result Failure(Error error) => new Result(error);
}