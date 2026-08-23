using Jellyfin.Plugin.GenAPIre.Configuration;

namespace Jellyfin.Plugin.GenAPIre.Tests
{
    public class PluginConfigurationTests
    {
        [Fact]
        public void DefaultBackendUrl_PointsToMobulumHost()
        {
            var config = new PluginConfiguration();

            Assert.Equal("https://genapire.mobulum.com", config.BackendUrl);
        }
    }
}
