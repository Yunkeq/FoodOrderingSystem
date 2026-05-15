using FluentAssertions;
using FoodOrderingSystem.Api.Contracts.Restaurants;
using FoodOrderingSystem.Api.Controllers;
using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;
using FoodOrderingSystem.Application.Restaurants.Commands.CreateRestaurant;
using FoodOrderingSystem.Application.Restaurants.Commands.DeleteRestaurant;
using FoodOrderingSystem.Application.Restaurants.Commands.UpdateRestaurant;
using FoodOrderingSystem.Application.Restaurants.Common;
using FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurantById;
using FoodOrderingSystem.Application.Restaurants.Queries.GetRestaurants;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace FoodOrderingSystem.UnitTests;

public sealed class RestaurantTests
{
    [Fact]
    public async Task Should_ReturnOk_WhenGetAllRestaurants()
    {
        // Arrange
        var items = new List<RestaurantDto>
        {
            new(Id: Guid.NewGuid(), Name: "A", City: "C1", IsOpen: true),
            new(Id: Guid.NewGuid(), Name: "B", City: "C2", IsOpen: false),
        };

        var getAllHandler = new Mock<IQueryHandler<GetRestaurantsQuery, IReadOnlyCollection<RestaurantDto>>>(MockBehavior.Strict);
        getAllHandler
            .Setup(h => h.Handle(It.IsAny<GetRestaurantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<IReadOnlyCollection<RestaurantDto>>.Success(items))
            .Verifiable(Times.Once);

        var controller = CreateController(getAllHandler: getAllHandler.Object);

        // Act
        var result = await controller.GetAll(CancellationToken.None);

        // Assert
        var ok = result.Result.Should().BeOfType<OkObjectResult>().Subject;

        var response = ok.Value.Should().BeAssignableTo<IReadOnlyCollection<RestaurantResponse>>().Subject;
        response.Should().HaveCount(2);

        response.Should().Contain(r =>
            r.Id == items[0].Id &&
            r.Name == "A" &&
            r.City == "C1" &&
            r.IsOpen == true);

        response.Should().Contain(r =>
            r.Id == items[1].Id &&
            r.Name == "B" &&
            r.City == "C2" &&
            r.IsOpen == false);
    }

    [Fact]
    public async Task Should_ReturnProblem404_WhenGetByIdRestaurantNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        var getByIdHandler = new Mock<IQueryHandler<GetRestaurantByIdQuery, RestaurantDto>>(MockBehavior.Strict);
        getByIdHandler
            .Setup(h => h.Handle(It.IsAny<GetRestaurantByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<RestaurantDto>.Failure(new Error(ErrorCode.RestaurantNotFound, "Not found")))
            .Verifiable(Times.Once);

        var controller = CreateController(getByIdHandler: getByIdHandler.Object);

        // Act
        var result = await controller.GetById(id, CancellationToken.None);

        // Assert
        var obj = result.Result.Should().BeOfType<ObjectResult>().Subject;
        obj.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var problem = obj.Value.Should().BeOfType<ProblemDetails>().Subject;
        problem.Status.Should().Be(StatusCodes.Status404NotFound);
        problem.Title.Should().Be(ErrorCode.RestaurantNotFound.ToString());
    }

    private static RestaurantsController CreateController(
        IQueryHandler<GetRestaurantsQuery, IReadOnlyCollection<RestaurantDto>>? getAllHandler = null,
        IQueryHandler<GetRestaurantByIdQuery, RestaurantDto>? getByIdHandler = null)
    {
        var createHandler = new Mock<ICommandHandler<CreateRestaurantCommand, Guid>>(MockBehavior.Strict);
        var updateHandler = new Mock<ICommandHandler<UpdateRestaurantCommand>>(MockBehavior.Strict);
        var deleteHandler = new Mock<ICommandHandler<DeleteRestaurantCommand>>(MockBehavior.Strict);

        var controller = new RestaurantsController(
            createHandler.Object,
            updateHandler.Object,
            deleteHandler.Object,
            getByIdHandler ?? Mock.Of<IQueryHandler<GetRestaurantByIdQuery, RestaurantDto>>(),
            getAllHandler ?? Mock.Of<IQueryHandler<GetRestaurantsQuery, IReadOnlyCollection<RestaurantDto>>>());

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext(),
        };

        return controller;
    }
}