using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Accounts;
using LykkeWallet.Areas.Exchange.Models;
using LykkeWallet.Controllers;

namespace LykkeWallet.Areas.Exchange.Controllers
{
    [Authorize]
    public class AssetsController : Controller
    {
        private readonly IAccountsRepository _accountsRepository;

        public AssetsController(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var kycResult = await this.GetKycStatus();
            if (kycResult != null)
                return kycResult;

            var clientId = this.GetClientId();

            var viewModel = new AssetsExchangeIndexViewModel
            {
                Accounts = await _accountsRepository.GetAccountsAsync(clientId)
            };

            return View(viewModel);
        }
    }

}