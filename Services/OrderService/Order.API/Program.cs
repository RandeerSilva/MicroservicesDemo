using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Application;
using Order.Application.Commands;
using Order.Infrastructure;
using DotNetCore.CAP;
using Order.Infrastructure.Repositories;
using Shared.Messaging.Abstractions;
using Shared.Messaging.Cap;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

var connectionString =
    builder.Configuration.GetConnectionString("orderdb")
    ?? throw new InvalidOperationException("Missing connection string 'orderdb' (expected from .NET Aspire).");

var natsConnectionString =
    builder.Configuration.GetConnectionString("demoNats")
    ?? throw new InvalidOperationException("Missing connection string 'nats' (expected from .NET Aspire).");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Add services to the container.

builder.Services.AddCap(capOption =>
{
    capOption.UseEntityFramework<AppDbContext>();
    capOption.UsePostgreSql(connectionString); // REQUIRED FOR OUTBOX

    capOption.UseNATS(n =>
    {
        n.Servers = natsConnectionString;
    });

    capOption.UseDashboard();
});
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IEventBus, CapEventBus>();
builder.Services.AddScoped<CreateOrderHandler>();
builder.Services.AddControllers();

// Swagger (Swashbuckle)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Order API", Version = "v1" });
});


var app = builder.Build();

// ✅ Run EF migrations at runtime
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
