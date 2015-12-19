using System;
using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Core.Clients;
using Core.Kyc;
using LkeServices.Clients;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    public class RegistrationController : ApiController
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;
        private readonly SrvClientManager _srvClientManager;
        private readonly IKycRepository _kycRepository;
        private readonly IPinSecurityRepository _puSecurityRepository;

        public RegistrationController(IClientAccountsRepository clientAccountsRepository, SrvClientManager srvClientManager, 
            IKycRepository kycRepository, IPinSecurityRepository puSecurityRepository)
        {
            _clientAccountsRepository = clientAccountsRepository;
            _srvClientManager = srvClientManager;
            _kycRepository = kycRepository;
            _puSecurityRepository = puSecurityRepository;
        }

        [Authorize]
        public async Task<ResponseModel<GetRegistrationStatusResponseModel>> Get()
        {
            var clientId = this.GetClientId();

            var kycStatus = await _kycRepository.GetKycStatusAsync(clientId);

            return ResponseModel<GetRegistrationStatusResponseModel>.CreateOk(
                new GetRegistrationStatusResponseModel
                {
                    KycStatus = kycStatus.ToResponseModel(),
                    PinIsEntered = await _puSecurityRepository.IsPinEntered(clientId)
                });
        }


        public async Task<ResponseModel<AccountsRegistrationResponseModel>> Post(AccountRegistrationModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return ResponseModel<AccountsRegistrationResponseModel>.CreateInvalidFieldError("email", Phrases.FieldShouldNotBeEmpty);

            if (!model.Email.IsValidEmail())
                return ResponseModel<AccountsRegistrationResponseModel>.CreateInvalidFieldError("email", Phrases.InvalidEmailFormat);

            if (string.IsNullOrEmpty(model.ContactPhone))
                return ResponseModel<AccountsRegistrationResponseModel>.CreateInvalidFieldError("contactphone", Phrases.FieldShouldNotBeEmpty);

            if (await _clientAccountsRepository.IsTraderWithEmailExistsAsync(model.Email))
                return ResponseModel<AccountsRegistrationResponseModel>.CreateInvalidFieldError("email", Phrases.ClientWithEmailIsRegistered);

            if (string.IsNullOrEmpty(model.Password))
                return ResponseModel<AccountsRegistrationResponseModel>.CreateInvalidFieldError("passowrd", Phrases.FieldShouldNotBeEmpty);

            try
            {
                if (string.IsNullOrEmpty(model.FullName))
                    model.FullName = model.FirstName + " " + model.LastName;

                var user = await _srvClientManager.RegisterClientAsync(model.Email, model.FullName, model.ContactPhone, model.Password, model.ClientInfo, this.GetIp());

                var token = await user.AuthenticateViaToken(model.ClientInfo);

                return ResponseModel<AccountsRegistrationResponseModel>.CreateOk(new AccountsRegistrationResponseModel {Token = token });
            }
            catch (Exception ex)
            {

                return ResponseModel<AccountsRegistrationResponseModel>.CreateInvalidFieldError("email", ex.StackTrace);
            }

        }
    }
}
