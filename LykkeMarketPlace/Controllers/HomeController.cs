using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Common;
using Core.Traders;
using LykkeMarketPlace.Models;
using LykkeMarketPlace.Services;
using LykkeMarketPlace.Strings;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace LykkeMarketPlace.Controllers
{

    public class HomeController : Controller
    {
        private readonly ITradersRepository _tradersRepository;



        public HomeController(ITradersRepository tradersRepository)
        {
            _tradersRepository = tradersRepository;
        }


        public ActionResult Index(string langId)
        {

            if (langId != null)
                this.SetLanguage(langId);

#if DEBUG
#else
            if (Request.Url.Scheme == "http")
                return Redirect("https://" + Request.Url.Authority);
#endif

            return View();
        }


        [HttpPost]
        public ActionResult SignInDialog()
        {
            var viewModel = new SignInDialogViewModel
            {
                Email = this.GetCookie(ControllerExt.EmailCookie)
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult SignUpDialog()
        {
            return View();
        }


        private JsonResult GetAuthenticatedJsonResult()
        {
            return this.JsonShowContentResultAndShowLoading("body", Url.Action("Index", "MarketPlace"));
        }

        [HttpPost]
        public async Task<ActionResult> SignIn(SignInModel model)
        {

            if (string.IsNullOrEmpty(model.Email))
                return this.JsonFailResult("#email", Phrases.FieldShouldNotBeEmpty);

            if (!model.Email.IsValidEmail())
                return this.JsonFailResult("#email", Phrases.PleaseTypeEmailHere);

            if (string.IsNullOrEmpty(model.Password))
                return this.JsonFailResult("#password", Phrases.FieldShouldNotBeEmpty);


            var trader = await _tradersRepository.AuthenticateAsync(model.Email, model.Password);

            if (trader == null)
                return this.JsonFailResult("#password", Phrases.InvalidUsernameOrPassword);

            this.AuthenticateUserViaOwin(trader);


            return GetAuthenticatedJsonResult();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(SignUpModel model)
        {

            if (string.IsNullOrEmpty(model.Email))
                return this.JsonFailResult("#email", Phrases.FieldShouldNotBeEmpty);

            if (!model.Email.IsValidEmail())
                return this.JsonFailResult("#email", Phrases.PleaseTypeEmailHere);


            if (await _tradersRepository.IsTraderWithEmailExistsAsync(model.Email))
                return this.JsonFailResult("#email", Phrases.UserWithEmalExists);

            if (string.IsNullOrEmpty(model.Password))
                return this.JsonFailResult("#password", Phrases.FieldShouldNotBeEmpty);


            if (string.IsNullOrEmpty(model.PasswordAgain))
                return this.JsonFailResult("#passwordAgain", Phrases.FieldShouldNotBeEmpty);

            if (model.Password != model.PasswordAgain)
                return this.JsonFailResult("#passwordAgain", Phrases.PasswordDoesNotMatch);


            var trader = await _tradersRepository.RegisterAsync(model, model.Password);

            this.AuthenticateUserViaOwin(trader);


            return GetAuthenticatedJsonResult();
        }


        public ActionResult LogOut()
        {
            this.SignOut();
            return Redirect(Request.Url.Scheme+"://"+ Request.Url.Authority);
        }

    }


    public static class ControllerExt
    {

        public const string EmailCookie = "email";

        public static JsonResult JsonFailResult(this Controller ctx, string divError, string message)
        {
            return new JsonResult { Data = new { Status = "Fail", divError, Result = message } };
        }

        public static JsonResult JsonHideDialog(this Controller ctx)
        {
            return new JsonResult { Data = new { Status = "HideDialog"} };
        }


        public static JsonResult JsonShowContentResultAndShowLoading(this Controller ctx, string divResult, string url, object prms = null)
        {
            if (prms == null)
                return new JsonResult { Data = new { Status = "Request", divResult, url, showLoading = true } };


            return new JsonResult { Data = new { Status = "Request", divResult, url, prms, showLoading = true } };
        }

        public static JsonResult JsonShowContentResult(this Controller ctx, string divResult, string url, object prms = null)
        {
            if (prms == null)
                return new JsonResult { Data = new { Status = "Request", divResult, url } };


            return new JsonResult { Data = new { Status = "Request", divResult, url, prms } };
        }


        public static void AuthenticateUserViaOwin(this Controller ctx, ITrader user)
        {
            var authManager = ctx.HttpContext.GetOwinContext().Authentication;
            var identity = MakeIdentity(user);

            authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);
            ctx.SetCookie(EmailCookie, user.Email, DateTime.UtcNow.AddYears(1));

        }

        private static ClaimsIdentity MakeIdentity(ITrader user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Id) };
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

        public static string GetTraderId(this Controller ctx)
        {
            return ctx.User.Identity.Name;
        }

        public static void SignOut(this Controller controller)
        {
            var authManager = controller.HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
        }

    }
}