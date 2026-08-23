using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.GenAPIre.Configuration
{
    /// <summary>
    /// Plugin configuration class persisted by Jellyfin.
    /// </summary>
    public class PluginConfiguration : BasePluginConfiguration
    {
        /// <summary>
        /// The base URL of the AudioMuse AI backend (include http:// or https://).
        /// </summary>
        public string BackendUrl { get; set; } = "https://genapire.mobulum.com";
    }
}