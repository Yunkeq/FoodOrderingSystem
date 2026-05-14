using FoodOrderingSystem.Application.Abstractions.Db;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.Orders.Common;
using Microsoft.EntityFrameworkCore;

namespace FoodOrderingSystem.Application.Orders.Queries.GetMyOrders;

public sealed class GetMyOrdersQueryHandler : IQueryHandler<GetMyOrdersQuery, IReadOnlyCollection<OrderDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetMyOrdersQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<IReadOnlyCollection<OrderDto>>> Handle(GetMyOrdersQuery query, CancellationToken cancellationToken)
    {
        var orders = await _dbContext.Orders
            .AsNoTracking()
            .Where(o => o.CustomerId == query.UserId)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderDto(
                o.Id,
                o.OrderDate,
                o.TotalPrice,
                o.OrderItems
                    .Select(oi => new OrderItemDto(
                        oi.MenuItemId,
                        oi.Name,
                        oi.Price,
                        oi.Quantity))
                    .ToList()))
            .ToListAsync(cancellationToken);

        return Result<IReadOnlyCollection<OrderDto>>.Success(orders);
    }
}