using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Tyoajanseuranta.Startup))]
namespace Tyoajanseuranta
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
