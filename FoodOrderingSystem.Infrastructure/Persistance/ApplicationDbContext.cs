using System.Reflection;
using FoodOrderingSystem.Domain.Entities;
using FoodOrderingSystem.Infrastructure.Persistance.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FoodOrderingSystem.Infrastructure.Persistance;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private const string DbSchema = "Db:Schema";
    private readonly string _schema;

    public ApplicationDbContext(IConfiguration configuration)
    {
        _schema = configuration[DbSchema] ?? throw new ArgumentException("Db schema is not specified.");
    }

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.HasDefaultSchema(_schema);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
