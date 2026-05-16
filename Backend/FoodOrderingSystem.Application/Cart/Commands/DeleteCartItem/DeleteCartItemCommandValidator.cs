using FluentValidation;

namespace FoodOrderingSystem.Application.Cart.Commands.DeleteCartItem;

public sealed class DeleteCartItemCommandValidator : AbstractValidator<DeleteCartItemCommand>
{
    public DeleteCartItemCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User id is required.");

        RuleFor(x => x.MenuItemId)
            .NotEmpty().WithMessage("Menu item id is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}