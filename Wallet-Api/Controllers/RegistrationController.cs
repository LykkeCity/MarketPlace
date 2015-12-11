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

        public async Task<ResponseModel> Post(string email, string firstname, string lastname, string contactphone, string password)
        {
            if (string.IsNullOrEmpty(email))
                return ResponseModel.CreateInvalidFieldError("email", Phrases.FieldShouldNotBeEmpty);

            if (!email.IsValidEmail())
                return ResponseModel.CreateInvalidFieldError("email", Phrases.InvalidEmailFormat);

            if (string.IsNullOrEmpty(firstname))
                return ResponseModel.CreateInvalidFieldError("firstname", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(lastname))
                return ResponseModel.CreateInvalidFieldError("lastname", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(contactphone))
                return ResponseModel.CreateInvalidFieldError("contactphone", Phrases.FieldShouldNotBeEmpty);

            if (await _clientAccountsRepository.IsTraderWithEmailExistsAsync(email))
                return ResponseModel.CreateInvalidFieldError("email", Phrases.ClientWithEmailIsRegistered);

            if (string.IsNullOrEmpty(password))
                return ResponseModel.CreateInvalidFieldError("passowrd", Phrases.FieldShouldNotBeEmpty);

            try
            {
                var user = await _srvClientManager.RegisterClientAsync(email, firstname, lastname, contactphone, password);
                this.AuthenticateUserViaOwin(user);
                return ResponseModel.CreateOk();
            }
            catch (Exception ex)
            {

                return ResponseModel.CreateInvalidFieldError("email", ex.StackTrace);
            }

        }
    }
}
