using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Core.Clients;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;

        public AuthController(IClientAccountsRepository clientAccountsRepository)
        {
            _clientAccountsRepository = clientAccountsRepository;
        }

        public async Task<object> Post(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                return FailFieldModel.Create("email", Phrases.FieldShouldNotBeEmpty);

            if (!email.IsValidEmail())
                return FailFieldModel.Create("email", Phrases.InvalidEmailFormat);

            if (string.IsNullOrEmpty(password))
                return FailFieldModel.Create("passowrd", Phrases.FieldShouldNotBeEmpty);

            var client = await _clientAccountsRepository.AuthenticateAsync(email, password);

            if (client == null)
                return FailFieldModel.Create("passowrd", Phrases.InvalidUsernameOrPassword);

            this.AuthenticateUserViaOwin(client);

            return OkResponseModel.Instance;
        }

    }

}
