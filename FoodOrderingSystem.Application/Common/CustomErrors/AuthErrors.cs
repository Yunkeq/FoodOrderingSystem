using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Common.CustomErrors;

public static class AuthErrors
{
    public static Error Unauthorized => new Error(ErrorCode.Unauthorized, "Unauthorized access.");
}
