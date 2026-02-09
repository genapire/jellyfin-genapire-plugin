#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.GenAPIre.Providers.GenAPIre
{
    /// <summary>
    /// GenAPIre Track metadata provider.
    /// </summary>
    public class GenapireTrackProvider : IRemoteMetadataProvider<Audio, SongInfo>, IHasOrder
    {
        private readonly ILogger<GenapireTrackProvider>? _logger;
        private readonly GenAPIreApiService _apiService;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenapireTrackProvider"/> class.
        /// </summary>
        public GenapireTrackProvider()
        {
            _logger = null;
            _apiService = new GenAPIreApiService();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenapireTrackProvider"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public GenapireTrackProvider(ILogger<GenapireTrackProvider> logger)
        {
            _logger = logger;
            _apiService = new GenAPIreApiService();
        }

        public string Name => "GenAPIre";

        public int Order => 0;

        public async Task<MetadataResult<Audio>> GetMetadata(SongInfo info, CancellationToken cancellationToken)
        {
            var result = new MetadataResult<Audio>();
            var artistName = info.AlbumArtists?.FirstOrDefault();

            if (string.IsNullOrWhiteSpace(artistName))
            {
                _logger?.LogDebug("GenAPIre: Missing artist name for track {TrackName}.", info.Name);
                return result;
            }

            _logger?.LogInformation("GenAPIre: Fetching genres for artist {Artist}", artistName);

            try
            {
                var genres = await _apiService.FetchArtistDataAsync(artistName, cancellationToken);

                if (genres != null && genres.Any())
                {
                    result.Item = new Audio();
                    genres.ForEach(genre => result.Item.AddGenre(genre));
                    result.HasMetadata = true;

                    _logger?.LogInformation("GenAPIre: Found genres [{Genres}] for artist {Artist}", string.Join(", ", genres), artistName);
                }
                else
                {
                    _logger?.LogDebug("GenAPIre: No genres found for artist {Artist}", artistName);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "GenAPIre: Error occurred while fetching track metadata.");
            }

            return result;
        }

        public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(SongInfo searchInfo, CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<RemoteSearchResult>());
        }

        public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
        {
            return Task.FromResult<HttpResponseMessage>(null!);
        }
    }
}
