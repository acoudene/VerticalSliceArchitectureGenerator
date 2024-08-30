var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Feature_Host>("feature-host");

builder.AddProject<Projects.Feature_WebApp>("feature-webapp");

builder.Build().Run();
