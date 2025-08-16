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
//          private const string BaseUrl = "https://genapire.online/artists";
          private const string BaseUrl = "http://localhost:8000/artists";
          private readonly HttpClient _httpClient;

          public GenAPIreApiService()
          {
              _httpClient = new HttpClient();
          }

          public async Task<List<string>> FetchGenresAsync(string artist, string album, CancellationToken cancellationToken)
          {
              if (string.IsNullOrWhiteSpace(artist) || string.IsNullOrWhiteSpace(album))
                  return null;

              var url = $"{BaseUrl}/{Uri.EscapeDataString(artist)}/albums/{Uri.EscapeDataString(album)}.json";

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
