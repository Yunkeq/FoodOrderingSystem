using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Common.CustomErrors;

public static class UserErrors
{
    public static Error UserAlreadyExists(string email) => new Error(ErrorCode.UserAlreadyExists, $"User with email {email} already exists.");
    public static Error UserNotFound(string email) => new Error(ErrorCode.UserNotFound, $"User with email {email} was not found.");
    public static Error InvalidUserCredentials(string email) => new Error(ErrorCode.InvalidUserCredentials, $"Invalid credentials for user with email {email}.");
}
