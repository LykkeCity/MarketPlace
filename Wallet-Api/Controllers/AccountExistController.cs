using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Core.Clients;

namespace Wallet_Api.Controllers
{
    public class AccountExistController : ApiController
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;

        public AccountExistController(IClientAccountsRepository clientAccountsRepository)
        {
            _clientAccountsRepository = clientAccountsRepository;
        }

        public async Task<bool?> Post(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            if (!email.IsValidEmail())
                return null;

            return await _clientAccountsRepository.IsTraderWithEmailExistsAsync(email);
        }
    }
}
