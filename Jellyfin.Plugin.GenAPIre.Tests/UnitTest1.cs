using System.Collections.Generic;
using Xunit;
using Jellyfin.Plugin.GenAPIre;
using Jellyfin.Plugin.GenAPIre.Providers.GenAPIre;

namespace Jellyfin.Plugin.GenAPIre.Tests;

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetOrDefault_WithExistingKey_ReturnsValue()
    {
        // Arrange
        var dict = new Dictionary<string, int> { { "key1", 42 } };

        // Act
        var result = dict.GetOrDefault("key1");

        // Assert
        Assert.Equal(42, result);
    }

    [Fact]
    public void GetOrDefault_WithMissingKey_ReturnsDefault()
    {
        // Arrange
        var dict = new Dictionary<string, int> { { "key1", 42 } };

        // Act
        var result = dict.GetOrDefault("key2");

        // Assert
        Assert.Equal(default(int), result);
    }

    [Fact]
    public void GetOrDefault_WithStringValues_ReturnsDefaultNull()
    {
        // Arrange
        var dict = new Dictionary<string, string> { { "key1", "value1" } };

        // Act
        var result = dict.GetOrDefault("nonexistent");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetOrDefault_WithEmptyDictionary_ReturnsDefault()
    {
        // Arrange
        var dict = new Dictionary<string, int>();

        // Act
        var result = dict.GetOrDefault("key");

        // Assert
        Assert.Equal(default(int), result);
    }
}

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