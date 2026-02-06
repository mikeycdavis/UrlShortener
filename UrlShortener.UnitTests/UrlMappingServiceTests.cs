using Microsoft.AspNetCore.Hosting;
using Moq;
using UrlShortener.Services;

namespace UrlShortener.UnitTests;

public class UrlMappingServiceTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly string _filePath;
    private readonly Mock<IWebHostEnvironment> _envMock;

    public UrlMappingServiceTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(_tempDirectory);

        _filePath = Path.Combine(_tempDirectory, "urls.txt");

        _envMock = new Mock<IWebHostEnvironment>();
        _envMock.Setup(e => e.ContentRootPath).Returns(_tempDirectory);
    }

    [Fact]
    public async Task GetUrlAsync_ReturnsUrl_WhenCodeExists()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath, "https://example.com/long/url abc", CancellationToken.None);

        var service = new UrlMappingService(_envMock.Object);

        // Act
        var result = await service.GetUrlAsync("abc");

        // Assert
        Assert.Equal("https://example.com/long/url", result);
    }

    [Fact]
    public async Task GetUrlAsync_IsCaseInsensitive()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath, "https://example.com/long/url AbC", CancellationToken.None);

        var service = new UrlMappingService(_envMock.Object);

        // Act
        var result = await service.GetUrlAsync("abc");

        // Assert
        Assert.Equal("https://example.com/long/url", result);
    }

    [Fact]
    public async Task GetUrlAsync_ReturnsNull_WhenCodeDoesNotExist()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath, "https://example.com/long/url abc", CancellationToken.None);

        var service = new UrlMappingService(_envMock.Object);

        // Act
        var result = await service.GetUrlAsync("missing");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetUrlAsync_IgnoresInvalidLines()
    {
        // Arrange
        await File.WriteAllTextAsync(_filePath,
            """
            https://example.com/valid abc
            invalidline
            https://another.com too many parts here
            """, 
            CancellationToken.None);

        var service = new UrlMappingService(_envMock.Object);

        // Act
        var result = await service.GetUrlAsync("abc");

        // Assert
        Assert.Equal("https://example.com/valid", result);
    }

    [Fact]
    public async Task GetUrlAsync_Throws_WhenFileDoesNotExist()
    {
        // Arrange
        var service = new UrlMappingService(_envMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<FileNotFoundException>(() =>
            service.GetUrlAsync("abc"));
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }

        GC.SuppressFinalize(this);
    }
}