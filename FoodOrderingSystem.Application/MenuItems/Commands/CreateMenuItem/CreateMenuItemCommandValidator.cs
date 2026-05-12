using FluentValidation;

namespace FoodOrderingSystem.Application.MenuItems.Commands.CreateMenuItem;

public sealed class CreateMenuItemCommandValidator : AbstractValidator<CreateMenuItemCommand>
{
    public CreateMenuItemCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Menu item name is required.")
            .MaximumLength(100).WithMessage("Menu item name must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.RestaurantId)
            .NotEmpty().WithMessage("Restaurant id is required.");
    }
}