using FoodOrderingSystem.Application.MenuItems.Common;

namespace FoodOrderingSystem.Application.Abstractions.Repositories;

public interface IMenuItemRepository
{
    Task<IReadOnlyCollection<MenuItemDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<IReadOnlyCollection<MenuItemDto>> GetByRestaurantIdAsync(Guid restaurantId, CancellationToken cancellationToken);
}