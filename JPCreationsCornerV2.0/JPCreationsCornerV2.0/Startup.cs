using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JPCreationsCornerV2._0.Startup))]
namespace JPCreationsCornerV2._0
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
