using System;
using System.Threading;
using Xunit;
using Jellyfin.Plugin.GenAPIre.Providers.GenAPIre;

namespace Jellyfin.Plugin.GenAPIre.Tests;

public class GenAPIreApiServiceTests
{
    [Fact]
    public async Task FetchGenresAsync_WithNullArtist_ReturnsNull()
    {
        // Arrange
        var service = new GenAPIreApiService();

        // Act
        var result = await service.FetchGenresAsync(null, "album", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FetchGenresAsync_WithEmptyArtist_ReturnsNull()
    {
        // Arrange
        var service = new GenAPIreApiService();

        // Act
        var result = await service.FetchGenresAsync("", "album", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FetchGenresAsync_WithWhitespaceArtist_ReturnsNull()
    {
        // Arrange
        var service = new GenAPIreApiService();

        // Act
        var result = await service.FetchGenresAsync("   ", "album", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FetchGenresAsync_WithNullAlbum_ReturnsNull()
    {
        // Arrange
        var service = new GenAPIreApiService();

        // Act
        var result = await service.FetchGenresAsync("artist", null, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FetchGenresAsync_WithEmptyAlbum_ReturnsNull()
    {
        // Arrange
        var service = new GenAPIreApiService();

        // Act
        var result = await service.FetchGenresAsync("artist", "", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FetchArtistDataAsync_WithNullArtist_ReturnsNull()
    {
        // Arrange
        var service = new GenAPIreApiService();

        // Act
        var result = await service.FetchArtistDataAsync(null, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task FetchArtistDataAsync_WithEmptyArtist_ReturnsNull()
    {
        // Arrange
        var service = new GenAPIreApiService();

        // Act
        var result = await service.FetchArtistDataAsync("", CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
