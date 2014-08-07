/*
 * Note: See the Startup.Auth.cs in App_Start to discover how to use this claims provider
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace SSW.Owin.AppClaimsModule.Example.ApplicationClaims
{

    public class ExampleClaimsProvider : AppClaimsProvider
    {
        public override IEnumerable<Claim> GetClaimsForPrincipal(ClaimsPrincipal principal)
        {
            const string issuer = "Example Claims Provider";
            var nameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var name = nameClaim == null ? "Unknown" : StripUsername(nameClaim.Value);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.DateOfBirth, new DateTime(1975, 5, 5).ToUniversalTime().ToString("o"),
                    ClaimValueTypes.DateTime, issuer),
                new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, issuer)
            };

            return claims;
        }

        private string StripUsername(string email)
        {
            if (email.Contains("@"))
            {
                return email.Substring(0, email.IndexOf("@", StringComparison.Ordinal));
            }
            return email;
        }
    }
}