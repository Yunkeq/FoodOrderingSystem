using FoodOrderingSystem.Api;
using FoodOrderingSystem.Application;
using FoodOrderingSystem.Infrastructure;
using FoodOrderingSystem.Infrastructure.Identity;
using FoodOrderingSystem.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddApi();
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();
    await IdentitySeedRunner.SeedAsync(app.Services);
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

// for integration test factory
public partial class Program
{
}