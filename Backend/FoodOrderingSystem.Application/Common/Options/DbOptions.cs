namespace FoodOrderingSystem.Application.Common.Options;

public sealed class DbOptions
{
    public required string ConnectionString { get; set; }
    public required string Schema { get; set; }
}
