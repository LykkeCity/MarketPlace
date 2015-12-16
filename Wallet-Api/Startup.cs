using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            ConfigureAuth(app);
        }
    }
}