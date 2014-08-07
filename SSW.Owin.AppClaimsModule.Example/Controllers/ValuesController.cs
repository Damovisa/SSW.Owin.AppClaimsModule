using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using SSW.Owin.AppClaimsAuthorization.WebApi;

namespace SSW.Owin.AppClaimsModule.Example.Controllers
{
    public class ValuesController : ApiController
    {
        [Route("api/UserOnly", Name = "UserOnly")]
        [HttpGet]
        [ClaimsAuthorize("Example.User")]
        public HttpResponseMessage UserOnly()
        {
            return Request.CreateResponse(new { Success = true, Message = "Users Only" });
        }


        [Route("api/AdminOnly", Name = "AdminOnly")]
        [HttpGet]
        [ClaimsAuthorize("Example.Admin", "Example.User")]
        public HttpResponseMessage AdminOnly()
        {
            return Request.CreateResponse(new {Success = true, Message = "Administrators Only"});
        }


        [Route("api/QldOnly", Name = "QldOnly")]
        [HttpGet]
        [ClaimsAuthorize("Queensland", ClaimType = ClaimTypes.StateOrProvince)]
        public HttpResponseMessage QldOnly()
        {
            return Request.CreateResponse(new { Success = true, Message = "Queensland Only" });
        }

    }
}
