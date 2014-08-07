using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SSW.Owin.AppClaimsModule.Example.Startup))]
namespace SSW.Owin.AppClaimsModule.Example
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
