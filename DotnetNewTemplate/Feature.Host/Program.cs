// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Api;
using Core.Api.Swaggers;
using Core.Data.MongoDb;
using Feature.Api;
using Feature.Host;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
  var builder = WebApplication.CreateBuilder(args);

  builder.Host.UseSerilog();

  Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

  /// <see cref="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/handle-errors?view=aspnetcore-7.0#problem-details"/>
  /// <seealso cref="https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-7.0&preserve-view=true#pds7"/>
  builder.Services.AddProblemDetails();

  /// Data
  builder.Services.ConfigureDataAdapters(builder.Configuration.GetSection(nameof(DatabaseSettings)));

  /// Add module to controller scanning, for clarty I have been redundant on controllers even if they share the same assembly 
  builder.Services.AddControllers(options =>
                  {
                    options.InputFormatters.Insert(0, JsonPatchHelper.GetJsonPatchInputFormatter());
                  })
                  .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(new AssemblyPart(typeof(EntityNameController).Assembly)))
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

  /// Cors
  const string frontEndBaseAddressKey = "FRONTEND_BASEADDRESS";
  string frontEndBaseAddress = builder.Configuration[frontEndBaseAddressKey] ?? string.Empty;
  if (string.IsNullOrWhiteSpace(frontEndBaseAddress))
    throw new InvalidOperationException($"Missing value for configuration key: {frontEndBaseAddressKey}");

  const string allowSpecificOrigins = "frontend";
  builder.Services.AddCors(options =>
  {
    options.AddPolicy(name: allowSpecificOrigins,
                      policy =>
                      {
                        policy.WithOrigins(frontEndBaseAddress);
                      });
  });

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

  app.UseCors(allowSpecificOrigins);

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