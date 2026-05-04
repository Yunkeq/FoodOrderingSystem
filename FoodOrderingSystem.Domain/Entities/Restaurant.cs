namespace FoodOrderingSystem.Domain.Entities;

public sealed class Restaurant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public bool IsOpen { get; set; }
}
