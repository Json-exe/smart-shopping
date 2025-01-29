using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<SmartShopping_Client>("client-app");

await builder.Build().RunAsync();