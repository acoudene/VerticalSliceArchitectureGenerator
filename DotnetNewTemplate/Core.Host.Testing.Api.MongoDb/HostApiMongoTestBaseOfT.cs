// Changelogs Date  | Author                | Description
// 2023-12-23       | Anthony Coudène       | Creation

using Core.Data.MongoDb;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MongoDb;
using Xunit.Abstractions;

namespace Core.Host.Testing.Api.MongoDb;

public abstract class HostApiMongoTestBase<TEntryPoint>
  : IClassFixture<WebApplicationFactory<TEntryPoint>>
  , IAsyncLifetime
  where TEntryPoint : class
{

  private readonly ITestOutputHelper _outputHelper;
  protected ITestOutputHelper OutputHelper { get { return _outputHelper; } }

  private WebApplicationFactory<TEntryPoint> _webApplicationFactory;
  protected WebApplicationFactory<TEntryPoint> WebApplicationFactory { get { return _webApplicationFactory; } } 

  //private const int Port = 27017;  

  /// <summary>
  /// Container  
  /// </summary>
  private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder()
      //.WithPortBinding(Port)
      .Build();

  protected MongoDbContainer MongoDbContainer { get => _mongoDbContainer; }

  protected string DatabaseName { get; private set; }

  public HostApiMongoTestBase(
    string databaseName,
    WebApplicationFactory<TEntryPoint> webApplicationFactory,
    ITestOutputHelper outputHelper)
  {
    DatabaseName = databaseName;
    _webApplicationFactory = webApplicationFactory;
    _outputHelper = outputHelper;
  }

  /// <summary>
  /// Initialize test class
  /// </summary>
  /// <returns></returns>
  public virtual async Task InitializeAsync()
  {
    await _mongoDbContainer.StartAsync();

    _webApplicationFactory = _webApplicationFactory.WithWebHostBuilder(builder =>
    {
      builder.ConfigureServices(services => services.Configure<DatabaseSettings>(Options =>
      {
        Options.ConnectionString = _mongoDbContainer.GetConnectionString();
        Options.DatabaseName = DatabaseName;
      }));
    });
  }

  /// <summary>
  /// Clean instance
  /// </summary>
  /// <returns></returns>
  public virtual async Task DisposeAsync()
  {
    await _mongoDbContainer.StopAsync();
    await _mongoDbContainer.DisposeAsync();
  }

  public virtual IHttpClientFactory CreateHttpClientFactory(string relativePath, WebApplicationFactoryClientOptions? givenOptions = null)
  {
    return TestHttpClientFactory<TEntryPoint>
      .CreateHttpClientFactory(_webApplicationFactory, relativePath, givenOptions);
  }
}
