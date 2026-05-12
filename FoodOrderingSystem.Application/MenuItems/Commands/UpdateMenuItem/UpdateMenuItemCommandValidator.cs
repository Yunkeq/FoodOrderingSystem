using FluentValidation;

namespace FoodOrderingSystem.Application.MenuItems.Commands.UpdateMenuItem;

public sealed class UpdateMenuItemCommandValidator : AbstractValidator<UpdateMenuItemCommand>
{
    public UpdateMenuItemCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Menu item id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Menu item name is required.")
            .MaximumLength(100).WithMessage("Menu item name must not exceed 100 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.RestaurantId)
            .NotEmpty().WithMessage("Restaurant id is required.");
    }
}