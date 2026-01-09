using System.Collections.Generic;
using Xunit;
using Jellyfin.Plugin.GenAPIre;

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
