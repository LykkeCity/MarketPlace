using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BackOffice.Startup))]
namespace BackOffice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
