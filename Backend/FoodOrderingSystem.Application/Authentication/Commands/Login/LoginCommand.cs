using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Authentication.Common;

namespace FoodOrderingSystem.Application.Authentication.Commands.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : ICommand<LoginDto>;