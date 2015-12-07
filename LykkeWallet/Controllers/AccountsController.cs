using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Core.Clients;
using LkeServices.Clients;
using LykkeWallet.Models;
using LykkeWallet.Strings;

namespace LykkeWallet.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientAccountsRepository _tradersRepository;
        private readonly SrvClientManager _srvClientManager;

        public ClientController(IClientAccountsRepository tradersRepository, SrvClientManager srvClientManager)
        {
            _tradersRepository = tradersRepository;
            _srvClientManager = srvClientManager;
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
        public ActionResult SignUpDialog(string email)
        {
            var viewModel = new SignUpViewModel
            {
                Email = email
            };
            return View(viewModel);
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

            if (string.IsNullOrEmpty(model.Phone))
                return this.JsonFailResult("#phone", Phrases.FieldShouldNotBeEmpty);

            var trader = await _srvClientManager.RegisterClientAsync(model, model.Password);

            this.AuthenticateUserViaOwin(trader);

            return GetAuthenticatedJsonResult();
        }

        [HttpPost]
        public ActionResult SignOut()
        {
            this.SignUserOut();
            return this.JsonRefreshRoot();
        }

        private JsonResult GetAuthenticatedJsonResult()
        {

            if (Request.Browser.IsMobileDevice)
                return this.JsonShowContentResult("#pamain", Url.Action("Index", "IdentityVerification"), null, new JsonResultExtParams { ShowLoading = true });

            return this.JsonShowContentResult("body", Url.Action("Index", "IdentityVerification"), null, new JsonResultExtParams { ShowLoading = true });
        }

    }
}