using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Clients;
using LykkeWallet.Models;
using LykkeWallet.Services;
using LykkeWallet.Strings;

namespace LykkeWallet.Controllers
{
    [Authorize]
    public class IdentityVerificationController : Controller
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;
        private readonly ISrvSmsConfirmator _srvSmsConfirmator;

        public IdentityVerificationController(IClientAccountsRepository clientAccountsRepository, ISrvSmsConfirmator srvSmsConfirmator)
        {
            _clientAccountsRepository = clientAccountsRepository;
            _srvSmsConfirmator = srvSmsConfirmator;
        }


        private ActionResult GetWalletIndexPage()
        {
            return this.JsonShowContentResult(WebSiteHelpers.MainContentDiv, Url.Action("Index", "Wallet"));
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {

            var clientId = this.GetClientId();

            var viewModel = new IdentityVerificationIndexViewModel
            {
                ClientAccount = await _clientAccountsRepository.GetByIdAsync(clientId)
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> CheckCode(string code)
        {
            var clientId = this.GetClientId();

            var verified = await _srvSmsConfirmator.CheckSmsConfirmation(clientId, code);

            if (!verified)
                return this.JsonFailResult("#code", Phrases.InvalidVerificaionCode);

            return GetWalletIndexPage();
        }


        [HttpPost]
        public async Task<ActionResult> SendCode()
        {
            var clientId = this.GetClientId();
            var clientAccount = await _clientAccountsRepository.GetByIdAsync(clientId);

            await _srvSmsConfirmator.SendSmsAsync(clientId);
            return this.JsonFailResult("#code", Phrases.VerificationSmsHasBeenSent+" "+ clientAccount.Phone);
        }
    }
}