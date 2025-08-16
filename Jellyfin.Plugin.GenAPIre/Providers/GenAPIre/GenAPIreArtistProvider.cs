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

using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Extensions;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Model.IO;

namespace Jellyfin.Plugin.GenAPIre.Providers.GenAPIre
{
    public class GenAPIreArtistProvider : IRemoteMetadataProvider<MusicArtist, ArtistInfo>, IHasOrder
    {
       private readonly ILogger<GenAPIreArtistProvider> _logger;
       private readonly GenAPIreApiService _apiService;

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
                    _logger.LogWarning("GenAPIreArtistProvider: Artist name is empty, skipping.");
                    return result;
                }

                var genres = await _apiService.FetchArtistDataAsync(artistName, cancellationToken);

                if (genres != null)
                {

                   if (result.Item == null)
                       result.Item = new MusicArtist();

                   result.Item.Name = artistName;
                   result.Item.Genres = genres.ToArray();
//                   result.Item.ProviderIds = info.ProviderIds;
                   result.Item.SetProviderId("GenAPIre", artistName);
//                   result.Item.SetProviderId(MetadataProvider.Custom, $"genapire-{artistName}");

                   result.Item.Path = info.Path;
                   result.HasMetadata = true;

                  _logger.LogDebug($"GenAPIreArtistProvider: Downloaded genres for artist '{artistName}': {string.Join(", ", genres)}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GenAPIreArtistProvider: Error fetching genres");
            }

            return result;
        }


        public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(ArtistInfo info, CancellationToken cancellationToken)
        {
            // Opcjonalnie implementacja wyszukiwania
            return Task.FromResult(Enumerable.Empty<RemoteSearchResult>());
        }

        public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private class ArtistDataResponse
        {
            public List<string> Genres { get; set; }
            // Dodaj inne pola odpowiednio, jeśli API zwraca np. biography, images, itp.
        }
    }
}
