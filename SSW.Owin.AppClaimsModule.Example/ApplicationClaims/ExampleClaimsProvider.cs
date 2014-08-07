/*
 * Note: See the Startup.Auth.cs in App_Start to discover how to use this claims provider
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.Owin;

namespace SSW.Owin.AppClaimsModule.Example.ApplicationClaims
{

    public class ExampleClaimsProvider : AppClaimsProvider
    {
        public override IEnumerable<Claim> GetClaimsForPrincipal(ClaimsPrincipal principal, IHeaderDictionary headers)
        {
            const string issuer = "Example Claims Provider";
            var nameClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            var name = nameClaim == null ? "Unknown" : StripUsername(nameClaim.Value);
            var claims = new List<Claim>
            {

                // ** Role Claims
                // Add User claim
                new Claim(ClaimTypes.Role, "Example.User", ClaimValueTypes.String, issuer),
                // Add Administrator claim
                new Claim(ClaimTypes.Role, "Example.Admin", ClaimValueTypes.String, issuer),

                // ** Additional Claims
                // Add State or Province claim
                new Claim(ClaimTypes.StateOrProvince, "Queensland", ClaimValueTypes.String, issuer),
                // Add new name and date of birth claims
                new Claim(ClaimTypes.Name, name, ClaimValueTypes.String, issuer),
                new Claim(ClaimTypes.DateOfBirth, new DateTime(1975, 5, 5).ToUniversalTime().ToString("o"), ClaimValueTypes.DateTime, issuer),
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