using BackOffice;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(BackOffice.Startup))]
namespace BackOffice
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
