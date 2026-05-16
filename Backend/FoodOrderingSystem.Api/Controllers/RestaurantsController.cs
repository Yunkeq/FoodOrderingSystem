using FoodOrderingSystem.Api.Contracts.Restaurants;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.Common.Security;
using FoodOrderingSystem.Application.Restaurants.Commands.CreateRestaurant;
using FoodOrderingSystem.Application.Restaurants.Commands.DeleteRestaurant;
using FoodOrderingSystem.Application.Restaurants.Commands.UpdateRestaurant;
using FoodOrderingSystem.Application.Restaurants.Common;
using FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurantById;
using FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodOrderingSystem.Api.Controllers;

[Route("api/restaurants")]
[ApiController]
public sealed class RestaurantsController : ControllerBase
{
    private readonly ICommandHandler<CreateRestaurantCommand, Guid> _createHandler;
    private readonly ICommandHandler<UpdateRestaurantCommand> _updateHandler;
    private readonly ICommandHandler<DeleteRestaurantCommand> _deleteHandler;
    private readonly IQueryHandler<GetRestaurantByIdQuery, RestaurantDto> _getByIdHandler;
    private readonly IQueryHandler<GetRestaurantsQuery, IReadOnlyCollection<RestaurantDto>> _getAllHandler;

    public RestaurantsController(
        ICommandHandler<CreateRestaurantCommand, Guid> createHandler,
        ICommandHandler<UpdateRestaurantCommand> updateHandler,
        ICommandHandler<DeleteRestaurantCommand> deleteHandler,
        IQueryHandler<GetRestaurantByIdQuery, RestaurantDto> getByIdHandler,
        IQueryHandler<GetRestaurantsQuery, IReadOnlyCollection<RestaurantDto>> getAllHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _getByIdHandler = getByIdHandler;
        _getAllHandler = getAllHandler;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<IReadOnlyCollection<RestaurantResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await _getAllHandler.Handle(new GetRestaurantsQuery(), cancellationToken);

        if (!result.IsSuccess)
        {
            return ProblemDetailsFromError(result.Error);
        }

        var response = result.Value!
            .Select(r => new RestaurantResponse(r.Id, r.Name, r.City, r.IsOpen))
            .ToList();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<RestaurantResponse>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _getByIdHandler.Handle(new GetRestaurantByIdQuery(id), cancellationToken);

        if (!result.IsSuccess)
        {
            return ProblemDetailsFromError(result.Error);
        }

        var r = result.Value!;
        return Ok(new RestaurantResponse(r.Id, r.Name, r.City, r.IsOpen));
    }

    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.AdminPriority)]
    public async Task<ActionResult> Create([FromBody] CreateRestaurantRequest request, CancellationToken cancellationToken)
    {
        var result = await _createHandler.Handle(new CreateRestaurantCommand(request.Name, request.City, request.IsOpen), cancellationToken);

        if (!result.IsSuccess)
        {
            return ProblemDetailsFromError(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, new { Id = result.Value });
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = AuthorizationPolicies.AdminPriority)]
    public async Task<ActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRestaurantRequest request, CancellationToken cancellationToken)
    {
        var result = await _updateHandler.Handle(new UpdateRestaurantCommand(id, request.Name, request.City, request.IsOpen), cancellationToken);

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
        var result = await _deleteHandler.Handle(new DeleteRestaurantCommand(id), cancellationToken);

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
            ErrorCode.RestaurantNotFound => StatusCodes.Status404NotFound,
            _ => throw new ArgumentException("Unexpected error code", nameof(error))
        };

        return Problem(
            statusCode: statusCode,
            title: error.ErrorCode.ToString(),
            detail: error.Message);
    }
}
