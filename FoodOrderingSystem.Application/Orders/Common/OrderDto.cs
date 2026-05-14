namespace FoodOrderingSystem.Application.Orders.Common;

public sealed record OrderDto(
    Guid Id,
    DateTime OrderDate,
    decimal TotalAmount,
    IReadOnlyCollection<OrderItemDto> Items);