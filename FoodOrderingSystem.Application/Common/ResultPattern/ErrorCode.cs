namespace FoodOrderingSystem.Application.Common.ResultPattern;

public enum ErrorCode
{
    None = 0,
    Validation,
    UserAlreadyExists,
    UserNotFound,
    InvalidUserCredentials,
    RestaurantNotFound,
    MenuItemNotFound,
    Unauthorized,
}
