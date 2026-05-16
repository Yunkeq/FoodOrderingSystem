using FoodOrderingSystem.Application.Abstractions.Messaging;

namespace FoodOrderingSystem.Application.MenuItems.Commands.DeleteMenuItem;

public sealed record DeleteMenuItemCommand(Guid Id) : ICommand;