using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Entities.Audio;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;
using Microsoft.Extensions.Logging;
using System.Net.Http;


using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Extensions;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Configuration;
using MediaBrowser.Model.IO;

namespace Jellyfin.Plugin.GenAPIre.Providers.GenAPIre
{
  public class GenAPIreProvider : IRemoteMetadataProvider<MusicAlbum, AlbumInfo>, IHasOrder
   {
       private readonly ILogger<GenAPIreProvider> _logger;
       private readonly GenAPIreApiService _apiService;

       public GenAPIreProvider(ILogger<GenAPIreProvider> logger)
       {
           _logger = logger;
           _apiService = new GenAPIreApiService();
       }

       /// <inheritdoc />
       public string Name => "GenAPIre";

       /// <inheritdoc />
       public int Order => 10; // numer kolejności providera, im niższy tym ważniejszy (MusicBrainz ma 0)

       public async Task<MetadataResult<MusicAlbum>> GetMetadata(AlbumInfo info, CancellationToken cancellationToken)
       {
           var result = new MetadataResult<MusicAlbum>();

           try
           {
               var album = info.Name;
               var artist = info.AlbumArtists?.FirstOrDefault();

                if (string.IsNullOrWhiteSpace(album) || string.IsNullOrWhiteSpace(artist))
                {
                    _logger.LogWarning($"GenAPIreProvider: brak nazwy albumu '{album}' lub artysty '{artist}', pomijam pobieranie.");
                    return result;
                }


               var genres = await _apiService.FetchGenresAsync(artist, album, cancellationToken);


               if (genres != null && genres.Count > 0)
               {

                   if (result.Item == null)
                       result.Item = new MusicAlbum();

                   result.Item.Name = album;

                   if (info.AlbumArtists != null)
                       result.Item.AlbumArtists = info.AlbumArtists;

                   result.Item.Genres = genres.ToArray();
                   result.HasMetadata = true;
                   result.Item.ProviderIds = info.ProviderIds;
                   result.Item.Path = info.Path;

                   _logger.LogInformation($"GenAPIreProvider: Pobrano gatunki dla albumu '{album}' artysty '{artist}': {string.Join(", ", genres)}");
               } else {
                _logger.LogInformation($"GenAPIreProvider: Brakuje gatunkow dla albumu '{album}' artysty '{artist}'");
               }
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, $"GenAPIreProvider: Błąd pobierania gatunków dla albumu.");
           }

           return result;
       }

       public Task<IEnumerable<RemoteSearchResult>> GetSearchResults(AlbumInfo info, CancellationToken cancellationToken)
       {
           // (Opcjonalnie) zaimplementuj jeśli chcesz obsługiwać wyszukiwanie albumów przez API
           return Task.FromResult(Enumerable.Empty<RemoteSearchResult>());
       }

       /// <inheritdoc />
       public Task<HttpResponseMessage> GetImageResponse(string url, CancellationToken cancellationToken)
       {
           throw new NotImplementedException();
       }
   }
}
