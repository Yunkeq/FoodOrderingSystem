namespace FoodOrderingSystem.Domain.Entities;

public sealed class Order
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public int TotalAmount { get; set; }
    public decimal TotalPrice { get; set; }
    public DateTime OrderDate { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
    public ApplicationUser? Customer { get; set; }
}
