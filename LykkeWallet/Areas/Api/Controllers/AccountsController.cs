using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Core.Clients;

namespace LykkeWallet.Areas.Api.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;

        public AccountsController(IClientAccountsRepository clientAccountsRepository)
        {
            _clientAccountsRepository = clientAccountsRepository;
        }

        [HttpGet]
        public async Task<ActionResult> Exist(string email)
        {
            var result = !string.IsNullOrEmpty(email) && (email.IsValidEmail() && await _clientAccountsRepository.IsTraderWithEmailExistsAsync(email.ToLower()));
            return Json(new {result}, JsonRequestBehavior.AllowGet);
        }

    }
}