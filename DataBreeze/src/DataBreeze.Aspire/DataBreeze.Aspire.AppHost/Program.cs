var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.DataBreeze_Grpc>("databreeze.grpc");

builder.AddProject<Projects.DataBreeze_LoadBalancer>("databreeze.loadbalancer");

builder.AddProject<Projects.ClientApp_Api>("clientapp.api");

builder.Build().Run();
