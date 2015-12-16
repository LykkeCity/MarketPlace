using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LykkeWallet.Startup))]
namespace LykkeWallet
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Localize();
            ConfigureAuth(app);
        }
    }
}
