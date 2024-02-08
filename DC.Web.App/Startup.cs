using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DC.Web.App.Startup))]
namespace DC.Web.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
