using Microsoft.EntityFrameworkCore;
using Stock.Application;
using Stock.Application.Services;
using Stock.Infrastructure;
using Stock.Infrastructure.Handlers;
using Stock.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

var connectionString =
    builder.Configuration.GetConnectionString("stockdb")
    ?? throw new InvalidOperationException("Missing connection string 'orderdb' (expected from .NET Aspire).");

var natsConnectionString =
    builder.Configuration.GetConnectionString("demoNats")
    ?? throw new InvalidOperationException("Missing connection string 'nats' (expected from .NET Aspire).");


builder.Services.AddDbContext<StockServiceDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCap(capOption =>
{
    capOption.UseEntityFramework<StockServiceDbContext>();
    capOption.UsePostgreSql(connectionString); // REQUIRED FOR OUTBOX

    capOption.UseNATS(n =>
    {
        n.Servers = natsConnectionString;
    });

    capOption.UseDashboard();
});
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<StockSubscriber>();
var app = builder.Build();
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<StockServiceDbContext>();
    await db.Database.MigrateAsync();
}
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
