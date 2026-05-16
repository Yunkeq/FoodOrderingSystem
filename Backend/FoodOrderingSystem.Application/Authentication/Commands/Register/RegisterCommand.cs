using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Common.ResultPattern;

namespace FoodOrderingSystem.Application.Authentication.Commands.Register;

public sealed record RegisterCommand(
    string Email,
    string Password) : ICommand<Guid>;
