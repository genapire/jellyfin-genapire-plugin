#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using MediaBrowser.Controller.Entities.Audio;

namespace Jellyfin.Plugin.GenAPIre.Providers.GenAPIre
{
    /// <summary>
    /// GenAPIre Artist metadata provider.
    /// </summary>
    public class GenAPIreArtistProvider : IRemoteMetadataProvider<MusicArtist, ArtistInfo>, IHasOrder
    {
       private readonly ILogger<GenAPIreArtistProvider>? _logger;
       private readonly GenAPIreApiService _apiService;

       /// <summary>
       /// Initializes a new instance of the <see cref="GenAPIreArtistProvider"/> class.
       /// </summary>
       public GenAPIreArtistProvider()
       {
           _logger = null;
           _apiService = new GenAPIreApiService();
       }

       /// <summary>
       /// Initializes a new instance of the <see cref="GenAPIreArtistProvider"/> class.
       /// </summary>
       /// <param name="logger">The logger.</param>
       public GenAPIreArtistProvider(ILogger<GenAPIreArtistProvider> logger)
       {
           _logger = logger;
           _apiService = new GenAPIreApiService();
       }

       /// <inheritdoc />
       public string Name => "GenAPIre";

       /// <inheritdoc />
       public int Order => 10;


        public async Task<MetadataResult<MusicArtist>> GetMetadata(ArtistInfo info, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<MusicArtist>();

            try
            {
                var artistName = info.Name;
                if (string.IsNullOrWhiteSpace(artistName))
                {
                    _logger?.LogWarning("GenAPIreArtistProvider: Artist name is empty, skipping.");
                    return result;
                }

                var genres = await _apiService.FetchArtistDataAsync(artistName, cancellationToken);

                if (genres != null)
                {

                   if (result.Item == null)
                       result.Item = new MusicArtist();

                   result.Item.Name = artistName;
                   result.Item.Genres = genres.ToArray();
                   result.Item.ProviderIds = info.ProviderIds;
//                   result.Item.SetProviderId("GenAPIre", artistName);
                   result.Item.SetProviderId(MetadataProvider.Custom, $"genapire-{artistName}");

                   result.Item.Path = info.Path;
                   result.HasMetadata = true;

                  _logger?.LogDebug($"GenAPIreArtistProvider: Downloaded genres for artist '{artistName}': {string.Join(", ", genres)}");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "GenAPIreArtistProvider: Error fetching genres");
            }

            return result;
        }


        public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(ArtistInfo info, CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<RemoteSearchResult>());
        }

        public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            return Task.FromResult<HttpResponseMessage>(null!);
        }
    }
}
