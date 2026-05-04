using System.Reflection;
using FoodOrderingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodOrderingSystem.Infrastructure.Persistance;

public sealed class ApplicationDbContext : DbContext
{
    private readonly string _schema;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _schema = configuration["Db:Schema"] ?? throw new ArgumentException("Db schema is not specified.");
    }

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(_schema);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
