using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Cart.Common;

namespace FoodOrderingSystem.Application.Cart.Queries.GetCart;

public sealed record GetCartQuery(Guid UserId) : IQuery<CartDto>;
