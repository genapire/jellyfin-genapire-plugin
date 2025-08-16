using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Jellyfin.Plugin.GenAPIre.Providers.GenAPIre
{
  public class GenAPIreApiService
      {
          private readonly HttpClient _httpClient;

          public GenAPIreApiService()
          {

           var config = Plugin.Instance?.Configuration;
                      var backendUrl = !string.IsNullOrWhiteSpace(config?.BackendUrl)
                          ? config.BackendUrl.TrimEnd('/')
                          : new Configuration.PluginConfiguration().BackendUrl.TrimEnd('/');

                      if (!Uri.IsWellFormedUriString(backendUrl, UriKind.Absolute))
                      {
                          throw new InvalidOperationException(
                              $"GenAPIre: BackendUrl is invalid ('{backendUrl}'). " +
                              "Please configure a valid absolute URL in Administration → Plugins → GenAPIre.");
                      }

                      _httpClient = new HttpClient { BaseAddress = new Uri(backendUrl) };
          }

        public async Task<List<string>> FetchArtistDataAsync(string artistName, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(artistName))
                return null;

            var url = $"/artists/{Uri.EscapeDataString(artistName)}.json";

            try
              {
                var response = await _httpClient.GetAsync(url, cancellationToken);
                if (!response.IsSuccessStatusCode)
                    return null;

                var json = await response.Content.ReadAsStringAsync();
                var genreObj = JsonSerializer.Deserialize<GenreResponse>(json);

                return genreObj?.genres ?? new List<string>();
              }
              catch
              {
                  return null;
              }
        }

          public async Task<List<string>> FetchGenresAsync(string artist, string album, CancellationToken cancellationToken)
          {
              if (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(album))
                  return null;

              var url = $"/artists/{Uri.EscapeDataString(artist)}/albums/{Uri.EscapeDataString(album)}.json";

              try
              {
                  var response = await _httpClient.GetAsync(url, cancellationToken);
                  if (!response.IsSuccessStatusCode)
                      return null;

                  var json = await response.Content.ReadAsStringAsync();
                  var genreObj = JsonSerializer.Deserialize<GenreResponse>(json);

                  return genreObj?.genres ?? new List<string>();
              }
              catch
              {
                  return null;
              }
          }

          public class GenreResponse
          {
              public List<string> genres { get; set; }
          }
      }
}
