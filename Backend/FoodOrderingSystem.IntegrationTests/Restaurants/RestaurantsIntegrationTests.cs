using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using FoodOrderingSystem.Api.Contracts.Auth;
using FoodOrderingSystem.Api.Contracts.Restaurants;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FoodOrderingSystem.IntegrationTests.Restaurants;

public sealed class RestaurantsIntegrationTests : BaseIntegrationTest
{
    public RestaurantsIntegrationTests(IntegrationTestFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        // Act
        var response = await HttpClient
            .GetAsync("/api/restaurants", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content
            .ReadFromJsonAsync<IReadOnlyCollection<RestaurantResponse>>(CancellationToken.None);
        body.Should().NotBeNull();
    }

    [Fact]
    public async Task GetById_ShouldReturn404_WhenRestaurantDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var response = await HttpClient.GetAsync($"/api/restaurants/{id}", CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(CancellationToken.None);
        problem.Should().NotBeNull();
        problem.Title.Should().Be("RestaurantNotFound");
    }

    [Fact]
    public async Task Create_ShouldReturn401_WhenUnauthorized()
    {
        // Arrange
        var request = new CreateRestaurantRequest(
            Name: "Integration Test Restaurant",
            City: "Test City",
            IsOpen: true);

        // Act
        var response = await HttpClient.PostAsJsonAsync("/api/restaurants", request, CancellationToken.None);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_Create_GetById_ShouldReturnCreatedAndProperRestaurant_WhenLogedInAsAdmin()
    {
        // login (cookies are persisted automatically)
        var loginRequest = new LoginRequest(
            Email: "admin@gmail.com",
            Password: "zaq1@WSX");

        var loginResponse = await HttpClient.PostAsJsonAsync("/api/auth/login", loginRequest, CancellationToken.None);
        loginResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // create restaurant
        var createRequest = new CreateRestaurantRequest(
            Name: $"Integration Admin Restaurant",
            City: "Admin City",
            IsOpen: true);

        var createResponse = await HttpClient.PostAsJsonAsync("/api/restaurants", createRequest, CancellationToken.None);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createBody = await createResponse.Content.ReadFromJsonAsync<CreateRestaurantResponse>(CancellationToken.None);
        createBody.Should().NotBeNull();
        createBody!.Id.Should().NotBeEmpty();

        // get by id
        var getResponse = await HttpClient.GetAsync($"/api/restaurants/{createBody.Id}", CancellationToken.None);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var restaurant = await getResponse.Content.ReadFromJsonAsync<RestaurantResponse>(CancellationToken.None);
        restaurant.Should().NotBeNull();
        restaurant!.Id.Should().Be(createBody.Id);
        restaurant.Name.Should().Be(createRequest.Name);
        restaurant.City.Should().Be(createRequest.City);
        restaurant.IsOpen.Should().Be(createRequest.IsOpen);
    }

    private sealed record CreateRestaurantResponse(Guid Id);
}