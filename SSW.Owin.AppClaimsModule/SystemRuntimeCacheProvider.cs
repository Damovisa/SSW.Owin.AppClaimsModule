using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Security.Claims;

namespace SSW.Owin.AppClaimsModule
{
    public class SystemRuntimeCacheProvider : ClaimsCacheProvider
    {
        private TimeSpan _cacheExpiry;
        private CacheItemPriority _cachePriority;
        private MemoryCache _cache;

        public SystemRuntimeCacheProvider(TimeSpan cacheExpiry, CacheItemPriority cachePriority = CacheItemPriority.Default)
        {
            _cacheExpiry = cacheExpiry;
            _cachePriority = cachePriority;
            _cache = new MemoryCache("SSW.Owin.AppClaimsModule");
        }

        public override void SaveClaims(string key, IEnumerable<Claim> claims)
        {
            _cache.Set(key, claims, new CacheItemPolicy { AbsoluteExpiration = DateTime.Now.Add(_cacheExpiry), Priority = _cachePriority });
        }

        public override bool HasKey(string key)
        {
            return _cache.Contains(key);
        }

        public override IEnumerable<Claim> GetClaims(string key)
        {
            return _cache.Get(key) as IEnumerable<Claim>;
        }
    }
}