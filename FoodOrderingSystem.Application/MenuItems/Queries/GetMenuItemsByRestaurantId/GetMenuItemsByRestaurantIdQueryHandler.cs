using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Abstractions.Repositories;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.MenuItems.Common;

namespace FoodOrderingSystem.Application.MenuItems.Queries.GetMenuItemsByRestaurantId;

public sealed class GetMenuItemsByRestaurantIdQueryHandler : IQueryHandler<GetMenuItemsByRestaurantIdQuery, IReadOnlyCollection<MenuItemDto>>
{
    private readonly IMenuItemRepository _menuItemRepository;

    public GetMenuItemsByRestaurantIdQueryHandler(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<Result<IReadOnlyCollection<MenuItemDto>>> Handle(GetMenuItemsByRestaurantIdQuery query, CancellationToken cancellationToken)
    {
        var items = await _menuItemRepository.GetByRestaurantIdAsync(query.RestaurantId, cancellationToken);
        return Result<IReadOnlyCollection<MenuItemDto>>.Success(items);
    }
}
