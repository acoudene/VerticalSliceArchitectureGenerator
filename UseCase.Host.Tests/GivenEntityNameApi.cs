namespace $safeprojectname$;

/// WARNING - for the moment, I don't have found a solution to reset settings like connexion string on a static test server
/// So be careful when changing settings, the same first settings will remain for server for all tests in this class even if this container is reset.
/// For example, don't change default port to reuse the same.
public class Given$ext_entityName$Api : HostApiMongoTestBase<Program>
{
  public Given$ext_entityName$Api(
    WebApplicationFactory<Program> webApplicationFactory,
    ITestOutputHelper output)
    : base($ext_entityName$, webApplicationFactory, output)
  {
  }

  private const string ApiPath = "/api";
  private const string ApiRelativePath = $"{ApiPath}/$ext_entityName$";

  [Fact]
  public async Task ThenGetAllAsync()
  {
    // Arrange
    var httpClientFactory = CreateHttpClientFactory(ApiRelativePath);
    var client = new Http$ext_entityName$Client(httpClientFactory);

    // Act
    var items = (await client.GetAllAsync());

    // Assert
  }

  [Fact]
  public async Task ThenCreateItemAsync()
  {
    // Arrange
    var httpClientFactory = CreateHttpClientFactory(ApiRelativePath);
    var client = new Http$ext_entityName$Client(httpClientFactory);
    var dtoToCreate = new $ext_entityName$Dto() 
    { 
      Id = Guid.NewGuid()
    };

    // Act
    HttpResponseMessage response = await client.CreateAsync(dtoToCreate, false);

    // Assert
    if (!response.IsSuccessStatusCode)
      OutputHelper.WriteLine(response.Content.ReadAsStringAsync().Result);
    response.EnsureSuccessStatusCode();
  }
}
