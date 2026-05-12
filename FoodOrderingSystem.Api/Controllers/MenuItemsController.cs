using FoodOrderingSystem.Api.Contracts.MenuItems;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.Common.Security;
using FoodOrderingSystem.Application.MenuItems.Commands.CreateMenuItem;
using FoodOrderingSystem.Application.MenuItems.Commands.DeleteMenuItem;
using FoodOrderingSystem.Application.MenuItems.Commands.UpdateMenuItem;
using FoodOrderingSystem.Application.MenuItems.Common;
using FoodOrderingSystem.Application.MenuItems.Queries.GetMenuItems;
using FoodOrderingSystem.Application.MenuItems.Queries.GetMenuItemsByRestaurantId;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Api.Controllers;

[Route("api/menu-items")]
[ApiController]
public sealed class MenuItemsController : ControllerBase
{
    private readonly ICommandHandler<CreateMenuItemCommand, Guid> _createHandler;
    private readonly ICommandHandler<UpdateMenuItemCommand> _updateHandler;
    private readonly ICommandHandler<DeleteMenuItemCommand> _deleteHandler;
    private readonly IQueryHandler<GetMenuItemsQuery, IReadOnlyCollection<MenuItemDto>> _getAllHandler;
    private readonly IQueryHandler<GetMenuItemsByRestaurantIdQuery, IReadOnlyCollection<MenuItemDto>> _getByRestaurantIdHandler;

    public MenuItemsController(
        ICommandHandler<CreateMenuItemCommand, Guid> createHandler,
        ICommandHandler<UpdateMenuItemCommand> updateHandler,
        ICommandHandler<DeleteMenuItemCommand> deleteHandler,
        IQueryHandler<GetMenuItemsQuery, IReadOnlyCollection<MenuItemDto>> getAllHandler,
        IQueryHandler<GetMenuItemsByRestaurantIdQuery, IReadOnlyCollection<MenuItemDto>> getByRestaurantIdHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _getAllHandler = getAllHandler;
        _getByRestaurantIdHandler = getByRestaurantIdHandler;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<MenuItemResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllHandler.Handle(new GetMenuItemsQuery(), cancellationToken);

        if (!result.IsSuccess)
        {
            return ProblemDetailsFromError(result.Error);
        }

        var response = result.Value!
            .Select(mi => new MenuItemResponse(mi.Id, mi.Name, mi.Price, mi.IsAvailable, mi.RestaurantId))
            .ToList();

        return Ok(response);
    }

    [HttpGet("by-restaurant/{restaurantId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<MenuItemResponse>>> GetByRestaurantId([FromRoute] Guid restaurantId, CancellationToken cancellationToken)
    {
        var result = await _getByRestaurantIdHandler.Handle(new GetMenuItemsByRestaurantIdQuery(restaurantId), cancellationToken);

        if (!result.IsSuccess)
        {
            return ProblemDetailsFromError(result.Error);
        }

        var response = result.Value!
            .Select(mi => new MenuItemResponse(mi.Id, mi.Name, mi.Price, mi.IsAvailable, mi.RestaurantId))
            .ToList();

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminPriority)]
    public async Task<ActionResult> Create([FromBody] CreateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _createHandler.Handle(
            new CreateMenuItemCommand(request.Name, request.Price, request.IsAvailable, request.RestaurantId),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return ProblemDetailsFromError(result.Error);
        }

        return Created(string.Empty, new { Id = result.Value });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPriority)]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMenuItemRequest request, CancellationToken cancellationToken)
    {
        var result = await _updateHandler.Handle(
            new UpdateMenuItemCommand(id, request.Name, request.Price, request.IsAvailable, request.RestaurantId),
            cancellationToken);

        if (!result.IsSuccess)
        {
            return ProblemDetailsFromError(result.Error);
        }

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPriority)]
    public async Task<ActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _deleteHandler.Handle(new DeleteMenuItemCommand(id), cancellationToken);

        if (!result.IsSuccess)
        {
            return ProblemDetailsFromError(result.Error);
        }

        return NoContent();
    }

    private ObjectResult ProblemDetailsFromError(Error error)
    {
        var statusCode = error.ErrorCode switch
        {
            ErrorCode.Validation => StatusCodes.Status400BadRequest,
            ErrorCode.MenuItemNotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status400BadRequest,
        };

        return Problem(
            statusCode: statusCode,
            title: error.ErrorCode.ToString(),
            detail: error.Message);
    }
}