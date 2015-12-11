using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Core.Clients;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    public class AccountExistController : ApiController
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;

        public AccountExistController(IClientAccountsRepository clientAccountsRepository)
        {
            _clientAccountsRepository = clientAccountsRepository;
        }

        public async Task<ResponseModel<AccountExistResultModel>> Get(string email)
        {
            if (string.IsNullOrEmpty(email))
                return ResponseModel<AccountExistResultModel>.CreateInvalidFieldError("email", Phrases.FieldShouldNotBeEmpty);

            if (!email.IsValidEmail())
                return ResponseModel<AccountExistResultModel>.CreateInvalidFieldError("email", Phrases.InvalidEmailFormat);

            return ResponseModel<AccountExistResultModel>.CreateOk(
                new AccountExistResultModel { IsEmailRegistered = await _clientAccountsRepository.IsTraderWithEmailExistsAsync(email)});
        }
    }
}
