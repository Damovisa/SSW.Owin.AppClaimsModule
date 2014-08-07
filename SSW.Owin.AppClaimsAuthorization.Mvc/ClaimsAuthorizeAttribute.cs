using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace SSW.Owin.AppClaimsAuthorization.Mvc
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] _permissions;
        private string _claimType;

        public ClaimsAuthorizeAttribute(params string[] permissions)
        {
            _permissions = permissions;
            _claimType = ClaimTypes.Role;
        }

        public virtual string ClaimType
        {
            get { return _claimType; }
            set { _claimType = value; }
        }
        
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = httpContext.User;
            if (user == null) return false;

            // make sure they have all permissions required
            return _permissions.Aggregate(true,
                (current, permission) => current && ((ClaimsPrincipal)user).HasClaim(_claimType, permission)
                );
        }
    }
}
