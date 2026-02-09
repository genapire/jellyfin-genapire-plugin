using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Jellyfin.Plugin.GenAPIre.Configuration;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;
using System.Globalization;

namespace Jellyfin.Plugin.GenAPIre
{
    /// <summary>
    /// GenAPIre plugin.
    /// </summary>
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Plugin"/> class.
        /// </summary>
        /// <param name="applicationPaths">The application paths.</param>
        /// <param name="xmlSerializer">The XML serializer.</param>
        public Plugin(
            IApplicationPaths applicationPaths,
            IXmlSerializer xmlSerializer)
            : base(applicationPaths, xmlSerializer)
        {
            Instance = this;
        }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        /// <returns>An HTTP client with user agent set.</returns>
        public HttpClient GetHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(Name, Version.ToString()));

            return httpClient;
        }

        /// <inheritdoc />
        public override string Name => Constants.PluginName;

        /// <inheritdoc />
        public override Guid Id => Guid.Parse(Constants.PluginGuid);

        /// <summary>
        /// Gets the plugin instance.
        /// </summary>
        public static Plugin Instance { get; private set; }

        /// <inheritdoc />
        public IEnumerable<PluginPageInfo> GetPages()
        {
            return
            [
                new PluginPageInfo
                {
                    Name = Name,
                    EmbeddedResourcePath = string.Format(CultureInfo.InvariantCulture, "{0}.Configuration.configPage.html", GetType().Namespace)
                }
            ];
        }
    }
}
