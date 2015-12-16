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

        public async Task<ResponseModel> Post(AccountRegistrationModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return ResponseModel.CreateInvalidFieldError("email", Phrases.FieldShouldNotBeEmpty);

            if (!model.Email.IsValidEmail())
                return ResponseModel.CreateInvalidFieldError("email", Phrases.InvalidEmailFormat);

            if (string.IsNullOrEmpty(model.FirstName))
                return ResponseModel.CreateInvalidFieldError("firstname", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(model.LastName))
                return ResponseModel.CreateInvalidFieldError("lastname", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(model.ContactPhone))
                return ResponseModel.CreateInvalidFieldError("contactphone", Phrases.FieldShouldNotBeEmpty);

            if (await _clientAccountsRepository.IsTraderWithEmailExistsAsync(model.Email))
                return ResponseModel.CreateInvalidFieldError("email", Phrases.ClientWithEmailIsRegistered);

            if (string.IsNullOrEmpty(model.Password))
                return ResponseModel.CreateInvalidFieldError("passowrd", Phrases.FieldShouldNotBeEmpty);

            try
            {
                var user = await _srvClientManager.RegisterClientAsync(model.Email, model.FirstName, model.LastName, model.ContactPhone, model.Password);
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
