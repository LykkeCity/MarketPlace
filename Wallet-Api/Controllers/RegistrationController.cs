using System;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Core.Clients;
using LkeServices.Clients;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    public class RegistrationController : ApiController
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;
        private readonly SrvClientManager _srvClientManager;

        public RegistrationController(IClientAccountsRepository clientAccountsRepository, SrvClientManager srvClientManager)
        {
            _clientAccountsRepository = clientAccountsRepository;
            _srvClientManager = srvClientManager;
        }

        public async Task<object> Post(string email, string firstname, string lastname, string contactphone, string passowrd)
        {
            if (string.IsNullOrEmpty(email))
                return FailFieldModel.Create("email", Phrases.FieldShouldNotBeEmpty);

            if (!email.IsValidEmail())
                return FailFieldModel.Create("email", Phrases.InvalidEmailFormat);

            if (string.IsNullOrEmpty(firstname))
                return FailFieldModel.Create("firstname", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(lastname))
                return FailFieldModel.Create("lastname", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(contactphone))
                return FailFieldModel.Create("contactphone", Phrases.FieldShouldNotBeEmpty);

            if (await _clientAccountsRepository.IsTraderWithEmailExistsAsync(email))
                return FailFieldModel.Create("email", Phrases.ClientWithEmailIsRegistered);

            if (string.IsNullOrEmpty(passowrd))
                return FailFieldModel.Create("passowrd", Phrases.FieldShouldNotBeEmpty);

            try
            {
                var user = await _srvClientManager.RegisterClientAsync(email, firstname, lastname, contactphone, passowrd);
                this.AuthenticateUserViaOwin(user);
                return OkResponseModel.Instance;
            }
            catch (Exception)
            {

                return FailFieldModel.Create("email", Phrases.TechnicalProblems);
            }

        }
    }
}
