using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.MenuItems.Common;

namespace FoodOrderingSystem.Application.MenuItems.Queries.GetMenuItemsByRestaurantId;

public sealed record GetMenuItemsByRestaurantIdQuery(Guid RestaurantId) : IQuery<IReadOnlyCollection<MenuItemDto>>;
