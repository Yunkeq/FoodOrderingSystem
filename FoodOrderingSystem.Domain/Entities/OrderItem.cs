namespace FoodOrderingSystem.Domain.Entities;

public sealed class OrderItem
{
    public Guid Id { get; set; }

    // I will store the name and price of the menu item at the time of order, to keep historical data even if the menu item changes later
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public Guid OrderId { get; set; }
    public Order? Order { get; set; }
    public Guid MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }
}
