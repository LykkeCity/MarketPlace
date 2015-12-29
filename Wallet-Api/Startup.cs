using Microsoft.Owin;
using Owin;
[assembly: OwinStartup(typeof(Wallet_Api.Startup))]
namespace Wallet_Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Localize();
            app.ConfigureLykkeAuth();
        }
    }
}