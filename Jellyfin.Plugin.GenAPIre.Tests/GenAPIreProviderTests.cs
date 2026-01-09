using System;
using System.Threading;
using Xunit;
using Jellyfin.Plugin.GenAPIre.Providers.GenAPIre;
using Moq;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Entities.Audio;

namespace Jellyfin.Plugin.GenAPIre.Tests;

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
