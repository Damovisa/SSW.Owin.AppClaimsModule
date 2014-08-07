using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin;

namespace SSW.Owin.AppClaimsModule
{
    public abstract class AppClaimsProvider
    {
        public abstract IEnumerable<Claim> GetClaimsForPrincipal(ClaimsPrincipal principal, IHeaderDictionary headers);

        public string GetPrincipalCacheKey(ClaimsPrincipal principal)
        {
            // try commonly-used identifying claims or a hash if none are available
            return
                GetClaimIfAvailable(principal, ClaimTypes.PrimarySid) ??
                GetClaimIfAvailable(principal, ClaimTypes.Sid) ??
                GetClaimIfAvailable(principal, ClaimTypes.Email) ??
                GetClaimIfAvailable(principal, ClaimTypes.Name) ??
                principal.GetHashCode().ToString(CultureInfo.InvariantCulture);
        }

        private string GetClaimIfAvailable(ClaimsPrincipal principal, string type)
        {
            var claim = principal.Claims.FirstOrDefault(c => c.Type == type);
            if (claim != null)
                return claim.Value;
            return null;
        }

    }
}