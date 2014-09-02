using System.Collections.Generic;
using System.Security.Claims;

namespace SSW.Owin.AppClaimsModule
{
    /// <summary>
    /// Additional ClaimsIdentity class specifically for storing Application Claims
    /// </summary>
    public class AppClaimsIdentity : ClaimsIdentity
    {
        public AppClaimsIdentity(IEnumerable<Claim> claims) : base(claims) { }

        public override string AuthenticationType
        {
            get { return "AppClaimsIdentity"; }
        }
    }
}