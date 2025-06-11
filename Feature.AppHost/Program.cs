// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

var builder = DistributedApplication.CreateBuilder(args);

const string databaseName = "feature";

var mongoContainer = builder.AddMongoDB("mongo")
  .WithLifetime(ContainerLifetime.Persistent);

var mongoDatabase = mongoContainer.AddDatabase(databaseName);

var featureHost = builder.AddProject<Projects.Feature_Host>("feature-host")
  .WithReference(mongoDatabase)
  .WaitFor(mongoDatabase);

builder.AddProject<Projects.Feature_WebApp>("feature-webapp")
  .WaitFor(featureHost);

/// dotnet tool install -g aspire.cli --prerelease
/// aspire publish
builder.AddDockerComposePublisher();

builder.Build().Run();
