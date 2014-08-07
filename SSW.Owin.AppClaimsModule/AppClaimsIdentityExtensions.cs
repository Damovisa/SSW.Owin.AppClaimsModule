using System.Collections.Generic;
using System.Security.Claims;
using Owin;

namespace SSW.Owin.AppClaimsModule
{
    public static class AppClaimsIdentityExtensions
    {
        public static void UseAdditionalClaimsIdentity(this IAppBuilder app,
            AppClaimsProvider claimsProvider,
            ClaimsCacheProvider cacheProvider = null)
        {
            app.Use((ctx, next) =>
            {
                var principal = ctx.Authentication.User;
                if (principal != null)
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
                        newClaims = claimsProvider.GetClaimsForPrincipal(principal);
                        if (cacheProvider != null)
                        {
                            var cacheKey = claimsProvider.GetPrincipalCacheKey(ctx.Authentication.User);
                            cacheProvider.SaveClaims(cacheKey, newClaims);
                        }
                    }

                    // add the new identity to the Owin Context
                    var additionalIdentity = new AppClaimsIdentity(newClaims);
                    ctx.Authentication.User.AddIdentity(additionalIdentity);
                }

                return next.Invoke();
            });
        }
    }
}