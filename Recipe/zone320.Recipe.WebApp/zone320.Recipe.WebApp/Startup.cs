using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(zone320.Recipe.WebApp.Startup))]
namespace zone320.Recipe.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
