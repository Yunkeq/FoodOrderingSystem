namespace FoodOrderingSystem.Application.Common.ResultPattern;

public sealed class Error
{
    public Error(ErrorCode errorCode, string message)
    {
        ErrorCode = errorCode;
        Message = message;
    }

    public static Error None => new Error(ErrorCode.None, string.Empty);

    public string Message { get; }
    public ErrorCode ErrorCode { get; }
}
