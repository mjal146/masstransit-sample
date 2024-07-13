using MassTransitDemo;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MassTransitDemoTest;

public class FileUploadControllerTests : IClassFixture<WebApplicationFactory<Startup>>
{
    private readonly WebApplicationFactory<Startup> _factory;
    private readonly HttpClient _client;

    public FileUploadControllerTests(WebApplicationFactory<Startup> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task UploadFile_ReturnsOk()
    {
        // Arrange
        var content = new MultipartFormDataContent();
        var bytes = "Id,Name\n1,one\n2,two"u8.ToArray(); 
        content.Add(new ByteArrayContent(bytes), "file", "test.csv");

        // Act
        var response = await _client.PostAsync("/api/fileupload/upload", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("File uploaded and processed", responseString);
    }

    [Fact]
    public async Task GetSwagger_ReturnsOk()
    {
        // Act
        var response = await _client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.Contains("My API", responseString);
    }
}