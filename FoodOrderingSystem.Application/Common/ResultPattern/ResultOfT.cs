using System.Diagnostics.CodeAnalysis;

namespace FoodOrderingSystem.Application.Common.ResultPattern;

public class Result<T> : Result
{
    protected Result(T? value, Error error)
        : base(error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value), "Success result cannot have a null value.");
        }

        return new Result<T>(value, Error.None);
    }

    public static new Result<T> Failure(Error error) => new Result<T>(default, error);
}
