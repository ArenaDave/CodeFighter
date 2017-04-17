using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HullMaker.Startup))]
namespace HullMaker
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
