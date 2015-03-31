using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TyoaikaApp.Startup))]
namespace TyoaikaApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
