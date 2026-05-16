using FluentValidation;

namespace FoodOrderingSystem.Application.Restaurants.Commands.DeleteRestaurant;

public sealed class DeleteRestaurantCommandValidator : AbstractValidator<DeleteRestaurantCommand>
{
    public DeleteRestaurantCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Restaurant id is required.");
    }
}