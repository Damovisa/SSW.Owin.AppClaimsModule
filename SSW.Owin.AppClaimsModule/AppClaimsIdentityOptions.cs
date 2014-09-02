using System.Runtime.InteropServices;

namespace SSW.Owin.AppClaimsModule
{
    public enum AppClaimsIdentityStrategy
    {
        AddToExistingIdentity,
        AddNewIdentity
    }

    public class AppClaimsIdentityOptions
    {
        public AppClaimsIdentityOptions(AppClaimsProvider claimsProvider, AppClaimsIdentityStrategy claimsIdentityStrategy, ClaimsCacheProvider cacheProvider = null)
        {
            ClaimsProvider = claimsProvider;
            ClaimsIdentityStrategy = claimsIdentityStrategy;
            CacheProvider = cacheProvider;
        }

        public AppClaimsProvider ClaimsProvider { get; set; }
        public AppClaimsIdentityStrategy ClaimsIdentityStrategy { get; set; }
        public ClaimsCacheProvider CacheProvider { get; set; }

    }
}