using System.Security.Claims;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.Orders.Commands.PlaceOrder;
using FoodOrderingSystem.Application.Orders.Common;
using FoodOrderingSystem.Application.Orders.Queries.GetMyOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Api.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize]
public sealed class OrderController : ControllerBase
{
    private readonly ICommandHandler<PlaceOrderCommand, Guid> _placeOrderHandler;
    private readonly IQueryHandler<GetMyOrdersQuery, IReadOnlyCollection<OrderDto>> _getMyOrdersHandler;

    public OrderController(
        ICommandHandler<PlaceOrderCommand, Guid> placeOrderHandler,
        IQueryHandler<GetMyOrdersQuery, IReadOnlyCollection<OrderDto>> getMyOrdersHandler)
    {
        _placeOrderHandler = placeOrderHandler;
        _getMyOrdersHandler = getMyOrdersHandler;
    }

    [HttpPost]
    public async Task<ActionResult> PlaceOrder(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var result = await _placeOrderHandler.Handle(new PlaceOrderCommand(userId), cancellationToken);

        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        return Created(string.Empty, new { OrderId = result.Value });
    }

    [HttpGet("mine")]
    public async Task<ActionResult<IReadOnlyCollection<OrderDto>>> GetMyOrders(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var result = await _getMyOrdersHandler.Handle(new GetMyOrdersQuery(userId), cancellationToken);

        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        return Ok(result.Value);
    }

    private Guid GetCurrentUserId()
    {
        var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (user is null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        return Guid.Parse(user);
    }

    private ObjectResult ToProblemDetails(Error error)
    {
        var statusCode = error.ErrorCode switch
        {
            ErrorCode.Validation => StatusCodes.Status400BadRequest,
            ErrorCode.MenuItemNotFound => StatusCodes.Status404NotFound,
            ErrorCode.CartIsEmpty => StatusCodes.Status409Conflict,
            ErrorCode.MenuItemIsNotAvailable => StatusCodes.Status400BadRequest,
            _ => throw new ArgumentException("Unexpected error code", nameof(error))
        };

        return Problem(
            statusCode: statusCode,
            title: error.ErrorCode.ToString(),
            detail: error.Message);
    }
}