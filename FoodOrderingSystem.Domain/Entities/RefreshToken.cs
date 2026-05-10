namespace FoodOrderingSystem.Domain.Entities;

public sealed class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
