using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Accounts;
using LykkeWallet.Areas.Accounts.Models;
using LykkeWallet.Controllers;

namespace LykkeWallet.Areas.Accounts.Controllers
{
    [Authorize]
    public class ListController : Controller
    {
        private readonly IAccountsRepository _accountsRepository;

        public ListController(IAccountsRepository accountsRepository)
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

            var viewModel = new AccountsIndexViewModel
            {
                Accounts = await _accountsRepository.GetAccountsAsync(clientId)
            };

            return View(viewModel);
        }
    }
}