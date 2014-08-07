using System.Security.Claims;
using System.Web.Mvc;
using SSW.Owin.AppClaimsAuthorization.Mvc;

namespace SSW.Owin.AppClaimsModule.Example.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ClaimsAuthorize("Example.User", "Example.Admin")]
        public ActionResult Administration()
        {
            return View();
        }

        [ClaimsAuthorize("Example.User")]
        public ActionResult ApplicationUser()
        {
            return View();
        }

        [ClaimsAuthorize("Queensland", ClaimType = ClaimTypes.StateOrProvince)]
        public ActionResult QueenslandOnly()
        {
            return View();
        }
    }
}