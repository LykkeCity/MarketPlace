using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(LykkeMarketPlace.Startup))]
namespace LykkeMarketPlace
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Localize();
            ConfigureAuth(app);
            app.MapSignalR();
        }
    }
}
