using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Extensions;
using Owin;

namespace SSW.Owin.AppClaimsModule
{
    public static class AppClaimsIdentityExtensions
    {
        /// <summary>
        /// Extension method to add new claims to the User in the Owin context.
        /// </summary>
        /// <param name="app">The Owin <see cref="IAppBuilder"/> providing the context</param>
        /// <param name="claimsProvider">Your class that will provide the additional claims, given a <see cref="ClaimsPrincipal"/> and <see cref="IHeaderDictionary"/></param>
        /// <param name="claimsIdentityStrategy">The strategy to use for adding claims. Whether to add to the existing identity (default) or add an additional one</param>
        /// <param name="cacheProvider">Optional provider for caching. For default Http Runtime caching, use <see cref="ClaimsCacheProvider.DefaultCacheProvider"/></param>
        public static void UseAppClaimsIdentityProvider(this IAppBuilder app,
            AppClaimsProvider claimsProvider,
            AppClaimsIdentityStrategy claimsIdentityStrategy = AppClaimsIdentityStrategy.AddToExistingIdentity,
            ClaimsCacheProvider cacheProvider = null)
        {
            var options = new AppClaimsIdentityOptions(claimsProvider, claimsIdentityStrategy, cacheProvider);
            app.Use<AppClaimsIdentityMiddleware>(app, options);
            app.UseStageMarker(PipelineStage.PostAuthenticate);
        }
      
    }
}