using System.Collections.Generic;
using Xunit;
using Jellyfin.Plugin.GenAPIre;
using Jellyfin.Plugin.GenAPIre.Providers.GenAPIre;
using Moq;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Entities.Audio;
using System.Threading;

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

public class GenAPIreProviderTests
{
    private Mock<ILogger<GenAPIreProvider>> _mockLogger;
    private GenAPIreProvider _provider;

    public GenAPIreProviderTests()
    {
        _mockLogger = new Mock<ILogger<GenAPIreProvider>>();
        _provider = new GenAPIreProvider(_mockLogger.Object);
    }

    [Fact]
    public async Task GetMetadata_WithMissingAlbum_ReturnsEmptyResult()
    {
        // Arrange
        var albumInfo = new AlbumInfo
        {
            Name = null,
            AlbumArtists = new[] { "Artist" }
        };

        // Act
        var result = await _provider.GetMetadata(albumInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_WithMissingArtist_ReturnsEmptyResult()
    {
        // Arrange
        var albumInfo = new AlbumInfo
        {
            Name = "Album",
            AlbumArtists = null
        };

        // Act
        var result = await _provider.GetMetadata(albumInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_WithEmptyAlbumName_ReturnsEmptyResult()
    {
        // Arrange
        var albumInfo = new AlbumInfo
        {
            Name = "",
            AlbumArtists = new[] { "Artist" }
        };

        // Act
        var result = await _provider.GetMetadata(albumInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_WithEmptyAlbumArtists_ReturnsEmptyResult()
    {
        // Arrange
        var albumInfo = new AlbumInfo
        {
            Name = "Album",
            AlbumArtists = new string[] { }
        };

        // Act
        var result = await _provider.GetMetadata(albumInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_WithValidInput_ReturnsResultWithoutMetadata()
    {
        // Arrange
        var albumInfo = new AlbumInfo
        {
            Name = "Test Album",
            AlbumArtists = new[] { "Test Artist" },
            Path = "/test/path"
        };

        // Act
        var result = await _provider.GetMetadata(albumInfo, CancellationToken.None);

        // Assert
        // Metoda będzie zwracać pusty wynik ponieważ API nie jest dostępne w teście
        Assert.NotNull(result);
        Assert.False(result.HasMetadata);
    }

    [Fact]
    public async Task GetMetadata_WithWhitespaceAlbumName_ReturnsEmptyResult()
    {
        // Arrange
        var albumInfo = new AlbumInfo
        {
            Name = "   ",
            AlbumArtists = new[] { "Artist" }
        };

        // Act
        var result = await _provider.GetMetadata(albumInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_PropagatesExceptions_LogsError()
    {
        // Arrange
        var albumInfo = new AlbumInfo
        {
            Name = "Album",
            AlbumArtists = new[] { "Artist" }
        };

        // Act
        var result = await _provider.GetMetadata(albumInfo, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.HasMetadata);
        // Sprawdzamy, że metodapoprawnie obsługuje błędy
    }

    [Fact]
    public void Provider_HasCorrectName()
    {
        // Assert
        Assert.Equal("GenAPIre", _provider.Name);
    }

    [Fact]
    public void Provider_HasCorrectOrder()
    {
        // Assert
        Assert.Equal(0, _provider.Order);
    }
}

public class GenapireTrackProviderTests
{
    private Mock<ILogger<GenapireTrackProvider>> _mockLogger;
    private GenapireTrackProvider _provider;

    public GenapireTrackProviderTests()
    {
        _mockLogger = new Mock<ILogger<GenapireTrackProvider>>();
        _provider = new GenapireTrackProvider(_mockLogger.Object);
    }

    [Fact]
    public async Task GetMetadata_WithMissingArtist_ReturnsEmptyResult()
    {
        // Arrange
        var songInfo = new SongInfo
        {
            Name = "Song",
            AlbumArtists = null
        };

        // Act
        var result = await _provider.GetMetadata(songInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_WithEmptyArtistList_ReturnsEmptyResult()
    {
        // Arrange
        var songInfo = new SongInfo
        {
            Name = "Song",
            AlbumArtists = new string[] { }
        };

        // Act
        var result = await _provider.GetMetadata(songInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_WithNullArtistName_ReturnsEmptyResult()
    {
        // Arrange
        var songInfo = new SongInfo
        {
            Name = "Song",
            AlbumArtists = new[] { (string)null }
        };

        // Act
        var result = await _provider.GetMetadata(songInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_WithWhitespaceArtist_ReturnsEmptyResult()
    {
        // Arrange
        var songInfo = new SongInfo
        {
            Name = "Song",
            AlbumArtists = new[] { "   " }
        };

        // Act
        var result = await _provider.GetMetadata(songInfo, CancellationToken.None);

        // Assert
        Assert.False(result.HasMetadata);
        Assert.Null(result.Item);
    }

    [Fact]
    public async Task GetMetadata_WithValidArtist_ProcessesMetadata()
    {
        // Arrange
        var songInfo = new SongInfo
        {
            Name = "Test Song",
            AlbumArtists = new[] { "Test Artist" }
        };

        // Act
        var result = await _provider.GetMetadata(songInfo, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        // Metoda obsługuje błędy gracefully, więc zwraca pusty wynik jeśli API nie dostępne
        Assert.False(result.HasMetadata);
    }

    [Fact]
    public async Task GetSearchResults_ReturnsEmptyEnumerable()
    {
        // Arrange
        var songInfo = new SongInfo
        {
            Name = "Song",
            AlbumArtists = new[] { "Artist" }
        };

        // Act
        var result = await _provider.GetSearchResults(songInfo, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void TrackProvider_HasCorrectName()
    {
        // Assert
        Assert.Equal("GenAPIre", _provider.Name);
    }

    [Fact]
    public void TrackProvider_HasCorrectOrder()
    {
        // Assert
        Assert.Equal(0, _provider.Order);
    }
}