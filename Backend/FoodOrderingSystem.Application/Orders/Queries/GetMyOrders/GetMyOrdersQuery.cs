using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Orders.Common;

namespace FoodOrderingSystem.Application.Orders.Queries.GetMyOrders;

public sealed record GetMyOrdersQuery(Guid UserId) : IQuery<IReadOnlyCollection<OrderDto>>;