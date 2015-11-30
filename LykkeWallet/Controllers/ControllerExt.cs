using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;
using Core.Clients;
using Core.Kyc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace LykkeWallet.Controllers
{
    public static class ControllerExt
    {

        public const string EmailCookie = "email";

        public static JsonResult JsonFailResult(this Controller ctx, string divError, string message)
        {
            return new JsonResult {Data = new {Status = "Fail", divError, Result = message}};
        }

        public static JsonResult JsonHideDialog(this Controller ctx)
        {
            return new JsonResult {Data = new {Status = "HideDialog"}};
        }


        public static JsonResult JsonShowContentResultAndShowLoading(this Controller ctx, string divResult, string url,
            object prms = null)
        {
            if (prms == null)
                return new JsonResult {Data = new {Status = "Request", divResult, url, showLoading = true}};

            return new JsonResult {Data = new {Status = "Request", divResult, url, prms, showLoading = true}};
        }


        public static JsonResult JsonRefreshRoot(this Controller ctx)
        {
            var host = ctx.Request.Url.OriginalString.ExtractWebSiteRoot();

            return new JsonResult { Data = new { Status = "Redirect", url = host } };
        }


        public static JsonResult JsonShowContentResult(this Controller ctx, string divResult, string url,
            object prms = null)
        {
            if (prms == null)
                return new JsonResult {Data = new {Status = "Request", divResult, url}};


            return new JsonResult {Data = new {Status = "Request", divResult, url, prms}};
        }


        public static void AuthenticateUserViaOwin(this Controller ctx, IClientAccount user)
        {
            var authManager = ctx.HttpContext.GetOwinContext().Authentication;
            var identity = MakeIdentity(user);

            authManager.SignIn(new AuthenticationProperties {IsPersistent = false}, identity);
            ctx.SetCookie(EmailCookie, user.Email, DateTime.UtcNow.AddYears(1));

        }

        private static ClaimsIdentity MakeIdentity(IClientAccount user)
        {
            var claims = new List<Claim> {new Claim(ClaimTypes.Name, user.Id)};
            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            return identity;
        }


        public static string GetCookie(this Controller ctx, string cookieName)
        {
            var cookie = ctx.Request.Cookies[cookieName];
            return cookie?.Value;
        }

        public static void SetCookie(this Controller ctx, string cookieName, string value, DateTime expires)
        {
            ctx.Response.Cookies.Add(new HttpCookie(cookieName, value) {Expires = expires});
        }

        public static string GetClientId(this Controller ctx)
        {
            return ctx.User.Identity.Name;
        }

        public static void SignUserOut(this Controller controller)
        {
            var authManager = controller.HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
        }


        public static async Task<JsonResult> GetKycStatus(this Controller controller)
        {
            var clientId = controller.GetClientId();

            var status = await Dependencies.KycRepository.GetKycStatusAsync(clientId);

            if (status != KycStatus.Ok)
                return controller.JsonShowContentResultAndShowLoading("#pamain",
                    controller.Url.Action("Index", "Page", new {area = "Kyc"}));

            return null;
        }

    }
}