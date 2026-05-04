using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodOrderingSystem.Application.Common.ResultPattern;

public class Result<T> : Result
{
    protected Result(T? value, Error error)
        : base(error)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new Result<T>(value, Error.None);
    public static new Result<T> Failure(Error error) => new Result<T>(default, error);
}
