using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BackOffice.Models;
using BackOffice.Services;
using BackOffice.Translates;
using Common;
using Core;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace BackOffice.Controllers
{


    public static class ControllerLangExtension
    {

        public static JsonResult JsonFailResult(this Controller contr, string message, string div)
        {
            return new JsonResult { Data = new { status = "Fail", msg=message, divError = div } };
        }

        public static JsonResult JsonRequestResult(this Controller contr, string div, string url, object model = null, bool putToHistory = false)
        {

            if (model == null)
                return new JsonResult { Data = new { div, refreshUrl = url} };

            var modelAsString = model as string ?? model.ToUrlParamString();
            return new JsonResult { Data = new { div, refreshUrl = url, prms = modelAsString } };
        }

        public static JsonResult JsonResultShowDialog(this Controller contr, string url, object model = null)
        {
            return new JsonResult { Data = new { url, model } };
        }

        public static JsonResult JsonResultReloadData(this Controller contr)
        {
            return new JsonResult { Data = new { Status = "Reload" } };
        }



        private const string OwnershipCookie = "DataOwnership";
        public static void SetDataOwnership(this Controller contr, string ownership)
        {
            if (string.IsNullOrEmpty(ownership))
                return;

            var newCookie = new HttpCookie(OwnershipCookie, ownership) { Expires = DateTime.UtcNow.AddYears(5) };
            contr.Response.SetCookie(newCookie);
        }

        public static string GetDataOwnership(this Controller contr)
        {
            return contr.Request.Cookies[OwnershipCookie]?.Value;
        }




        public const string LangCookie = "Language";

        //public static ITranslation UserLanguage(this HttpRequestBase request)
        //{
        //    var langCookie = request.Cookies[LangCookie];
        //    var langId = langCookie == null ? DetectLangIdByHeader(request) : langCookie.Value;
        //    return PhraseList.GetTranslations(langId);

        //}


        //public static void SetThread(ITranslation translation)
        //{
        //    Thread.CurrentThread.CurrentCulture = translation.Culture;
        //    Thread.CurrentThread.CurrentUICulture = translation.Culture;
        //}



        public static string GetDomain(this Controller contr)
        {
            return UrlUtils.ExtractDomain(contr.Request.Url.AbsoluteUri);
        }

        public static string SessionCoocke => "Session";

        public static string GetSession(this Controller contr)
        {
            var sessionCooke = contr.HttpContext.Request.Cookies[SessionCoocke];
            if (sessionCooke != null)
                return sessionCooke.Value;

            var sessionId = Guid.NewGuid().ToString();
            var newCookie = new HttpCookie(SessionCoocke, sessionId) { Expires = DateTime.UtcNow.AddYears(5) };
            contr.Response.SetCookie(newCookie);
            return sessionId;
        }

        public static HttpBrowserCapabilitiesBase GetBrowser(this Controller contr)
        {
            return contr.Request.RequestContext.HttpContext.Request.Browser;
        }

        public static string GetBrowserInfo(this Controller contr)
        {
            return
                contr.Request.RequestContext.HttpContext.Request.Browser.Browser + " " +
                contr.Request.RequestContext.HttpContext.Request.Browser.Version + " " +
                contr.Request.RequestContext.HttpContext.Request.Browser.Platform;
        }

        public static string GetIp(this Controller contr)
        {
            return contr.Request.UserHostAddress;
        }

        public static IPAddress MyIpAddress(this Controller contr)
        {
            return IPAddress.Parse(GetIp(contr));
        }



        public static string GetUserId(this Controller controller)
        {
            return controller.User.Identity.Name;
        }

    }


    public class HomeController : Controller
    {
        private readonly IBrowserSessionsRepository _browserSessionsRepository;
        private readonly IBackOfficeUsersRepository _backOfficeUsersRepository;

        public HomeController(IBrowserSessionsRepository browserSessionsRepository, IBackOfficeUsersRepository backOfficeUsersRepository)
        {
            _browserSessionsRepository = browserSessionsRepository;
            _backOfficeUsersRepository = backOfficeUsersRepository;
        }

        public async Task<ActionResult> Index(string langId)
        {


            if (langId != null)
                this.SetLanguage(langId);

            var sessionId = this.GetSession();

            var viewModel = new IndexPageModel
            {
                BrowserSession = await _browserSessionsRepository.GetSessionAsync(sessionId)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Authenticate(AuthenticateModel data)
        {
            if (string.IsNullOrEmpty(data.Username))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#username");

            if (string.IsNullOrEmpty(data.Password))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#password");

            var user =
                await
                    _backOfficeUsersRepository.AuthenticateAsync(data.Username, data.Password);

            if (user == null)
                return this.JsonFailResult(Phrases.InvalidUsernameOrPassword, "#password");
            var sessionId = this.GetSession();


            await _browserSessionsRepository.SaveSessionAsync(sessionId, user.Id);

            SignIn(user);

            var divResult = Request.Browser.IsMobileDevice ? "#pamain" : "body";

            return this.JsonRequestResult(divResult, Url.Action(nameof(BackOfficeController.Layout), "BackOffice"));
        }

        private static ClaimsIdentity MakeIdentity(IBackOfficeUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id),
                new Claim(ClaimTypes.Email, user.Id)
            };


            var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

            return identity;
        }


        private void SignIn(IBackOfficeUser user)
        {
            var authManager = HttpContext.GetOwinContext().Authentication;

            var identity = MakeIdentity(user);

            authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
        }


    }
}