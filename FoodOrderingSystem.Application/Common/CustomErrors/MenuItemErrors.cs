using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Common.CustomErrors;

public static class MenuItemErrors
{
    public static Error MenuItemNotFound(Guid id) => new Error(ErrorCode.MenuItemNotFound, $"Menu item '{id}' was not found.");
}