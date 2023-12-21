using Core.Api;
using Core.Api.Swaggers;
using Core.Data.MongoDb;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Serilog;

using $ext_safeprojectname$.Api;
using $ext_safeprojectname$.Data.MongoDb.Repositories;
using $ext_safeprojectname$.Data.Repositories;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
  var builder = WebApplication.CreateBuilder(args);

  builder.Host.UseSerilog();

  /// <see cref="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-errors?view=aspnetcore-7.0#problem-details"/>
  /// <seealso cref="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-7.0&preserve-view=true#pds7"/>
  builder.Services.AddProblemDetails();

  // https://kevsoft.net/2022/02/18/setting-up-mongodb-to-use-standard-guids-in-csharp.html
#pragma warning disable CS0618
  BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
  //BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
#pragma warning restore CS0618
  try
  {
    BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
  }
  catch (BsonSerializationException)
  {
    // Just to let integration tests work
  }

  builder.Services.AddControllers(options =>
  {
    options.InputFormatters.Insert(0, JsonPatchHelper.GetJsonPatchInputFormatter());
  });

  // Add services to the container.

  /// Connexion strings
  builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));

  /// Form templates API
  builder.Services.AddScoped<IMongoContext, MongoContext>();
  builder.Services.AddScoped<I$ext_entityName$Repository, $ext_entityName$Repository > ();

  /// Add module to controller scanning, for clarty I have been redundant on controllers even if they share the same assembly 
  builder.Services.AddControllers()
                  .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(new AssemblyPart(typeof($ext_entityName$Controller).Assembly)))
                  ;

  /// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
  builder.Services.AddEndpointsApiExplorer();
  builder.Services.AddSwaggerGen(options =>
  {
    options.DocumentFilter<JsonPatchDocumentFilter>();
    options.CustomSchemaIds(type => SwashbuckleSchemaHelper.GetIncrementalSchemaId(type));
    options.UseAllOfForInheritance();
    //options.OperationFilter<HttpResultsOperationFilter>();
  });

  //const string allowSpecificOrigins = "frontend";
  //builder.Services.AddCors(options =>
  //{
  //  options.AddPolicy(name: allowSpecificOrigins,
  //                    policy =>
  //                    {
  //                      policy.WithOrigins("https://localhost:7297");
  //                    });
  //});

  var app = builder.Build();

  app.UseSerilogRequestLogging();

  /// <see cref="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-7.0&preserve-view=true#exception-handler-page"/>
  app.UseExceptionHandler();
  /// <see cref="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-7.0&preserve-view=true#usestatuscodepages"/>
  app.UseStatusCodePages();

  // Configure the HTTP request pipeline.
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();

    /// <see cref="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-7.0&preserve-view=true#developer-exception-page"/>
    app.UseDeveloperExceptionPage();
  }

  app.UseHttpsRedirection();

  app.UseAuthorization();

  //app.UseCors(allowSpecificOrigins);

  app.MapControllers();

  app.Run();
}
catch (Exception ex)
{
  Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
  Log.CloseAndFlush();
}

// Just to let integration tests work
public partial class Program { }