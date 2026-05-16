using FoodOrderingSystem.Application.Abstractions.Messaging;
using FoodOrderingSystem.Application.Authentication.Common;

namespace FoodOrderingSystem.Application.Authentication.Commands.Refresh;

public sealed record RefreshCommand(string RefreshToken) : ICommand<LoginDto>;
