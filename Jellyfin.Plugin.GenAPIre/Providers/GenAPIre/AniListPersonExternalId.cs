using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Providers;
using MediaBrowser.Model.Entities;
using MediaBrowser.Model.Providers;

namespace Jellyfin.Plugin.GenAPIre.Providers.GenAPIre
{
    public class AniListPersonExternalId : IExternalId
    {
        public bool Supports(IHasProviderIds item)
            => item is Person;

        public string ProviderName
            => "GenAPIre";

        public string Key
            => ProviderNames.GenAPIre;

        public ExternalIdMediaType? Type
            => ExternalIdMediaType.Person;

        public string UrlFormatString
            => "https://anilist.co/staff/{0}/";
    }
}
