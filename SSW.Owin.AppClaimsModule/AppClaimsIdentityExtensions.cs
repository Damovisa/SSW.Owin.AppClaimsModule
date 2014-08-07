using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.Owin;
using Owin;

namespace SSW.Owin.AppClaimsModule
{
    public static class AppClaimsIdentityExtensions
    {
        /// <summary>
        /// Extension method to add a new identity to the Owin context.
        /// The primary User.Identity will be untouched, but User will have an additional identity you can access, and additional claims.
        /// </summary>
        /// <param name="app">The Owin <see cref="IAppBuilder"/> providing the context</param>
        /// <param name="claimsProvider">Your class that will provide the additional claims</param>
        /// <param name="cacheProvider">Optional provider for caching. For default Http Runtime caching, use <see cref="ClaimsCacheProvider.DefaultCacheProvider"/></param>
        public static void UseAppClaimsIdentityProvider(this IAppBuilder app,
            AppClaimsProvider claimsProvider,
            ClaimsCacheProvider cacheProvider = null)
        {
            app.Use((ctx, next) =>
            {
                var principal = ctx.Authentication.User;
                if (principal != null)
                {
                    var newClaims = GetClaims(claimsProvider, cacheProvider, ctx, principal);

                    // add the new identity to the Owin Context
                    var additionalIdentity = new AppClaimsIdentity(newClaims);
                    ctx.Authentication.User.AddIdentity(additionalIdentity);
                }

                return next.Invoke();
            });
        }

        /// <summary>
        /// Extension method to add a new identity to the Owin context.
        /// The primary User.Identity will have claims added to it directly.
        /// </summary>
        /// <param name="app">The Owin <see cref="IAppBuilder"/> providing the context</param>
        /// <param name="claimsProvider">Your class that will provide the additional claims</param>
        /// <param name="cacheProvider">Optional provider for caching. For default Http Runtime caching, use <see cref="ClaimsCacheProvider.DefaultCacheProvider"/></param>
        public static void UseAppClaimsAdditiveProvider(this IAppBuilder app,
          AppClaimsProvider claimsProvider,
          ClaimsCacheProvider cacheProvider = null)
        {
            app.Use((ctx, next) =>
            {
                var principal = ctx.Authentication.User;
                if (principal != null)
                {
                    var newClaims = GetClaims(claimsProvider, cacheProvider, ctx, principal);

                    // add the claims to the existing identity
                    ((ClaimsIdentity)ctx.Authentication.User.Identity).AddClaims(newClaims);
                }

                return next.Invoke();
            });
        }

        private static IEnumerable<Claim> GetClaims(
            AppClaimsProvider claimsProvider,
            ClaimsCacheProvider cacheProvider,
            IOwinContext ctx, ClaimsPrincipal principal)
        {
            IEnumerable<Claim> newClaims = null;

            // see if the claims are already in cache
            if (cacheProvider != null)
            {
                var cacheKey = claimsProvider.GetPrincipalCacheKey(ctx.Authentication.User);
                if (cacheProvider.HasKey(cacheKey))
                {
                    newClaims = cacheProvider.GetClaims(cacheKey);
                }
            }

            // if no claims have been retrieved (either no cacheprovider or not in cache)
            if (newClaims == null)
            {
                newClaims = claimsProvider.GetClaimsForPrincipal(principal, ctx.Request.Headers);
                if (cacheProvider != null)
                {
                    var cacheKey = claimsProvider.GetPrincipalCacheKey(ctx.Authentication.User);
                    cacheProvider.SaveClaims(cacheKey, newClaims);
                }
            }
            return newClaims;
        }

      
    }
}