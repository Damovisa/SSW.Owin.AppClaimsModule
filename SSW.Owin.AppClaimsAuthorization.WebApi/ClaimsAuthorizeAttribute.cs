using System.Linq;
using System.Security.Claims;
using System.Web.Http.Controllers;

namespace SSW.Owin.AppClaimsAuthorization.WebApi
{
    public class ClaimsAuthorizeAttribute : System.Web.Http.AuthorizeAttribute
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

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var user = actionContext.RequestContext.Principal;
            if (user == null) return false;

            // make sure they have all permissions required
            return _permissions.Aggregate(true,
                (current, permission) => current && ((ClaimsPrincipal)user).HasClaim(_claimType, permission)
                );
        }
    }
}
