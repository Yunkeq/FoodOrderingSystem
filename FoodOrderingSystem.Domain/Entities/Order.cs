namespace FoodOrderingSystem.Domain.Entities;

public sealed class Order
{
    public Guid Id { get; set; }
    public int TotalAmount { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();

    // should contain customer id, info etc, but for simplicity I will skip it
}
