var builder = DistributedApplication.CreateBuilder(args);

var nats = builder.AddNats("DemoNats", port: 4226)
    .WithJetStream();

var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var orderdb = postgres.AddDatabase("orderdb");
var stockdb = postgres.AddDatabase("stockdb");


builder.AddProject<Projects.Stock_API>("stock-api")
    .WithReference(stockdb)
    .WithReference(nats)
    .WaitFor(stockdb);

builder.AddProject<Projects.Order_API>("order-api")
    .WithReference(orderdb)
    .WithReference(nats)
    .WaitFor(orderdb)
    .WaitFor(nats);

builder.Build().Run();