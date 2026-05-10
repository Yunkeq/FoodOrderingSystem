using FoodOrderingSystem.Api;
using FoodOrderingSystem.Application;
using FoodOrderingSystem.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddUserSecrets<Program>(optional: true);

if (builder.Environment.IsDevelopment())
{
}

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
