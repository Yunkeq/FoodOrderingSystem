using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Abstractions.Repositories;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.MenuItems.Common;

namespace FoodOrderingSystem.Application.MenuItems.Queries.GetMenuItems;

public sealed class GetMenuItemsQueryHandler : IQueryHandler<GetMenuItemsQuery, IReadOnlyCollection<MenuItemDto>>
{
    private readonly IMenuItemRepository _menuItemRepository;

    public GetMenuItemsQueryHandler(IMenuItemRepository menuItemRepository)
    {
        _menuItemRepository = menuItemRepository;
    }

    public async Task<Result<IReadOnlyCollection<MenuItemDto>>> Handle(GetMenuItemsQuery query, CancellationToken cancellationToken)
    {
        var items = await _menuItemRepository.GetAllAsync(cancellationToken);
        return Result<IReadOnlyCollection<MenuItemDto>>.Success(items);
    }
}