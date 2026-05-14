using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Common.CustomErrors;

public static class OrderErrors
{
    public static Error CartIsEmpty() => new Error(ErrorCode.CartIsEmpty, "Cannot create an order because cart is empty.");
}