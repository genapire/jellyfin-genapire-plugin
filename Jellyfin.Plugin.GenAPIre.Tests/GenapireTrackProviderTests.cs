using System;
using System.Threading;
using Xunit;
using Jellyfin.Plugin.GenAPIre.Providers.GenAPIre;
using Moq;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Controller.Entities.Audio;

namespace Jellyfin.Plugin.GenAPIre.Tests;

public class GenapireTrackProviderTests
{
    private GenapireTrackProvider _provider;

    public GenapireTrackProviderTests()
    {
        _provider = new GenapireTrackProvider();
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
            AlbumArtists = new List<string>()
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
