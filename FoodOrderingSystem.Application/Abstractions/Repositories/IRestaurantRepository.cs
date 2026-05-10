using FoodOrderingSystem.Application.Restaurants.Common;

namespace FoodOrderingSystem.Application.Abstractions.Repositories;

public interface IRestaurantRepository
{
    Task<IReadOnlyCollection<RestaurantDto>> GetAllAsync(CancellationToken cancellationToken);
    Task<RestaurantDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
