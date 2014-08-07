using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web;
using System.Web.Caching;

namespace SSW.Owin.AppClaimsModule
{
    public abstract class ClaimsCacheProvider
    {
        public abstract void SaveClaims(string key, IEnumerable<Claim> claims);

        public abstract bool HasKey(string key);

        public abstract IEnumerable<Claim> GetClaims(string key);

        public static ClaimsCacheProvider DefaultCacheProvider
        {
            get
            {
                var cache = HttpRuntime.Cache;
                cache.Add("key", "value", null, DateTime.Now.AddHours(1), TimeSpan.FromMinutes(30),
                    CacheItemPriority.Default, null);
                return new HttpRuntimeCache(TimeSpan.FromHours(1), CacheItemPriority.Default);
            }
        }
    }
}