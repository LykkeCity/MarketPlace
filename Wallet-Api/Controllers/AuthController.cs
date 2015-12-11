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

        public async Task<ResponseModel> Post(AuthenticateModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return ResponseModel.CreateInvalidFieldError("email", Phrases.FieldShouldNotBeEmpty);

            if (!model.Email.IsValidEmail())
                return ResponseModel.CreateInvalidFieldError("email", Phrases.InvalidEmailFormat);

            if (string.IsNullOrEmpty(model.Password))
                return ResponseModel.CreateInvalidFieldError("passowrd", Phrases.FieldShouldNotBeEmpty);

            var client = await _clientAccountsRepository.AuthenticateAsync(model.Email, model.Password);

            if (client == null)
                return ResponseModel.CreateInvalidFieldError("passowrd", Phrases.InvalidUsernameOrPassword);

            this.AuthenticateUserViaOwin(client);

            return ResponseModel.CreateOk();
        }

    }

}
