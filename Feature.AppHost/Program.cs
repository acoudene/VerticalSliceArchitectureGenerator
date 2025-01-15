var builder = DistributedApplication.CreateBuilder(args);

const string databaseName = "feature";

var mongoContainer = builder.AddMongoDB("mongo")
  .WithLifetime(ContainerLifetime.Persistent);

var mongoDatabase = mongoContainer.AddDatabase(databaseName);

builder.AddProject<Projects.Feature_Host>("feature-host")
  .WithReference(mongoDatabase)
  .WaitFor(mongoDatabase);

builder.AddProject<Projects.Feature_WebApp>("feature-webapp");

builder.Build().Run();
