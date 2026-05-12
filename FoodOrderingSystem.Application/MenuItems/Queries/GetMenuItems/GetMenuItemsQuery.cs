using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.MenuItems.Common;

namespace FoodOrderingSystem.Application.MenuItems.Queries.GetMenuItems;

public sealed record GetMenuItemsQuery() : IQuery<IReadOnlyCollection<MenuItemDto>>;