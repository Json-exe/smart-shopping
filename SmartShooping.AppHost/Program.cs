using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<BarcodeScannerLiveApp>("client-app");

await builder.Build().RunAsync();