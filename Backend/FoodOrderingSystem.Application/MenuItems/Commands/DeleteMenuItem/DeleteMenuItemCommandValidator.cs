using FluentValidation;

namespace FoodOrderingSystem.Application.MenuItems.Commands.DeleteMenuItem;

public sealed class DeleteMenuItemCommandValidator : AbstractValidator<DeleteMenuItemCommand>
{
    public DeleteMenuItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Menu item id is required.");
    }
}