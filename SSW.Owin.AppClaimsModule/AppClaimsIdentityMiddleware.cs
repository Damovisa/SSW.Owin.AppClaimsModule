using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Owin;

namespace SSW.Owin.AppClaimsModule
{
    
    public class AppClaimsIdentityMiddleware
    {
        private readonly Func<IDictionary<string, object>, Task> _next;
        private readonly AppClaimsIdentityOptions _options;

        public AppClaimsIdentityMiddleware(Func<IDictionary<string, object>, Task> next, IAppBuilder app, AppClaimsIdentityOptions options)
        {
            app.CreateLogger<AppClaimsIdentityMiddleware>();

            if (next == null)
            {
                throw new ArgumentNullException("next");
            }
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            _next = next;
            _options = options;
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            IOwinContext context = new OwinContext(environment);

            var principal = context.Authentication.User;
            if (principal != null)
            {
                // retrieve the claims using the claims provider
                var newClaims = GetClaims(_options.ClaimsProvider, _options.CacheProvider, context, principal);

                if (_options.ClaimsIdentityStrategy == AppClaimsIdentityStrategy.AddToExistingIdentity)
                {
                    // add the claims to the existing identity
                    ((ClaimsIdentity) context.Authentication.User.Identity).AddClaims(newClaims);
                }
                else if (_options.ClaimsIdentityStrategy == AppClaimsIdentityStrategy.AddNewIdentity)
                {
                    // add the new identity to the Owin Context
                    var additionalIdentity = new AppClaimsIdentity(newClaims);
                    context.Authentication.User.AddIdentity(additionalIdentity);
                }
            }

            return _next(environment);
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
