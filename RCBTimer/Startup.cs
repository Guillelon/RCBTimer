using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RCBTimer.Startup))]
namespace RCBTimer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
