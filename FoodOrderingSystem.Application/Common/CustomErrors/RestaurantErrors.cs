using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Common.CustomErrors;

public static class RestaurantErrors
{
    public static Error RestaurantNotFound(Guid id) => new Error(ErrorCode.RestaurantNotFound, $"Restaurant '{id}' was not found.");
}
