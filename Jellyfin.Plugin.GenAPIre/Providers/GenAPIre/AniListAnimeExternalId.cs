using MediaBrowser.Controller.Entities.TV;
using MediaBrowser.Controller.Entities.Movies;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.GenAPIre.Providers.GenAPIre
{
    public class AniListAnimeExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item)
            => item is Series || item is Movie;

        public string ProviderName
            => "GenAPIre";

        public string Key
            => ProviderNames.GenAPIre;

        public ExternalIdMediaType? Type
            => ExternalIdMediaType.Series;

        public string UrlFormatString
            => "https://anilist.co/anime/{0}/";
    }
}
