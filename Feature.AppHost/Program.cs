var builder = DistributedApplication.CreateBuilder(args);

// Temporary independant Mongo coupled only by port and database name but a coupling will be done.
const int port = 27017;
const string databaseName = "feature";

var mongoContainer = builder.AddMongoDB("mongo", port);
mongoContainer.AddDatabase(databaseName);

builder.AddProject<Projects.Feature_Host>("feature-host");

builder.AddProject<Projects.Feature_WebApp>("feature-webapp");

builder.Build().Run();
