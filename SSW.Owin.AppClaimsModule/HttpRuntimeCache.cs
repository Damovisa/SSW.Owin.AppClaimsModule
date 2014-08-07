using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Caching;

namespace SSW.Owin.AppClaimsModule
{
    public class HttpRuntimeCache : ClaimsCacheProvider
    {
        private readonly TimeSpan _cacheExpiry;
        private readonly CacheItemPriority _cachePriority;

        public HttpRuntimeCache(TimeSpan cacheExpiry, CacheItemPriority cachePriority)
        {
            _cacheExpiry = cacheExpiry;
            _cachePriority = cachePriority;
        }

        public override void SaveClaims(string key, IEnumerable<Claim> claims)
        {
            if (HttpRuntime.Cache.Get(key) != null)
            {
                HttpRuntime.Cache[key] = claims;
            }
            else
            {
                var expiry = DateTime.Now.Add(_cacheExpiry);
                HttpRuntime.Cache.Add(key, claims, null, expiry, Cache.NoSlidingExpiration, _cachePriority, null);
            }
        }

        public override bool HasKey(string key)
        {
            return HttpRuntime.Cache.Get(key) != null;
        }

        public override IEnumerable<Claim> GetClaims(string key)
        {
            var claims = HttpRuntime.Cache.Get(key);
            if (claims == null)
                return null;

            return claims as IEnumerable<Claim>;
        }
    }
}