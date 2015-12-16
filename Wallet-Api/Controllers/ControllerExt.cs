using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using Core.Clients;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Wallet_Api.Controllers
{

    public static class ControllerExt
    {

        public static void AuthenticateUserViaOwin(this ApiController ctx, IClientAccount user)
        {
            var authManager = ctx.ControllerContext.Request.GetOwinContext().Authentication;
            var identity = MakeIdentity(user);
            authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
        }

        private static ClaimsIdentity MakeIdentity(IClientAccount user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id) };
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            return identity;
        }

        public static string GetClientId(this ApiController ctx)
        {
            return ApiDependencies.GetIdentity(ctx);
        }
    }
}