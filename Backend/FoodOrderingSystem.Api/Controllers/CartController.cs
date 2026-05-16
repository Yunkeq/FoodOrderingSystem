using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Cart.Commands.AddToCart;
using FoodOrderingSystem.Application.Cart.Commands.ClearCart;
using FoodOrderingSystem.Application.Cart.Commands.DeleteCartItem;
using FoodOrderingSystem.Application.Cart.Common;
using FoodOrderingSystem.Application.Cart.Queries.GetCart;
using FoodOrderingSystem.Application.Common.ResultPattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodOrderingSystem.Api.Controllers;

[Route("api/cart")]
[ApiController]
[Authorize]
public class CartController : ControllerBase
{
    private readonly IQueryHandler<GetCartQuery, CartDto> _getCartHandler;
    private readonly ICommandHandler<AddToCartCommand> _addToCartHandler;
    private readonly ICommandHandler<DeleteCartItemCommand> _deleteCartItemHandler;
    private readonly ICommandHandler<ClearCartCommand> _clearCartHandler;

    public CartController(
        IQueryHandler<GetCartQuery, CartDto> getCartHandler,
        ICommandHandler<AddToCartCommand> addToCartHandler,
        ICommandHandler<DeleteCartItemCommand> deleteCartItemHandler,
        ICommandHandler<ClearCartCommand> clearCartHandler)
    {
        _getCartHandler = getCartHandler;
        _addToCartHandler = addToCartHandler;
        _deleteCartItemHandler = deleteCartItemHandler;
        _clearCartHandler = clearCartHandler;
    }

    [HttpGet]
    public async Task<ActionResult> GetCart(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var result = await _getCartHandler.Handle(new GetCartQuery(userId), cancellationToken);

        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("add/{menuItemId:guid}")]
    public async Task<ActionResult> AddToCart(Guid menuItemId, int quantity, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var result = await _addToCartHandler.Handle(
            new AddToCartCommand(
                UserId: userId,
                MenuItemId: menuItemId,
                Quantity: quantity),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        return NoContent();
    }

    [HttpPost("remove/{menuItemId:guid}")]
    public async Task<ActionResult> RemoveFromCart(Guid menuItemId, int quantity, CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var result = await _deleteCartItemHandler.Handle(
            new DeleteCartItemCommand(
                UserId: userId,
                MenuItemId: menuItemId,
                Quantity: quantity),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        return NoContent();
    }

    [HttpPost("clear")]
    public async Task<ActionResult> ClearCart(CancellationToken cancellationToken)
    {
        var userId = GetCurrentUserId();

        var result = await _clearCartHandler.Handle(new ClearCartCommand(userId), cancellationToken);

        if (!result.IsSuccess)
        {
            return ToProblemDetails(result.Error);
        }

        return NoContent();
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
            ErrorCode.MenuItemIsNotAvailable => StatusCodes.Status400BadRequest,
            _ => throw new ArgumentException("Unexpected error code", nameof(error))
        };

        return Problem(
            statusCode: statusCode,
            title: error.ErrorCode.ToString(),
            detail: error.Message);
    }
}