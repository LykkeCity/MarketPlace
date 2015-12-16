﻿using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Core.Clients;
using Core.Kyc;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    public class AuthController : ApiController
    {
        private readonly IClientAccountsRepository _clientAccountsRepository;
        private readonly IKycRepository _kycRepository;
        private readonly IPinSecurityRepository _pinSecurityRepository;

        public AuthController(IClientAccountsRepository clientAccountsRepository, IKycRepository kycRepository, 
            IPinSecurityRepository pinSecurityRepository)
        {
            _clientAccountsRepository = clientAccountsRepository;
            _kycRepository = kycRepository;
            _pinSecurityRepository = pinSecurityRepository;
        }

        public async Task<ResponseModel<AuthenticateResponseModel>> Post(AuthenticateModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return ResponseModel<AuthenticateResponseModel>.CreateInvalidFieldError("email", Phrases.FieldShouldNotBeEmpty);

            if (!model.Email.IsValidEmail())
                return ResponseModel<AuthenticateResponseModel>.CreateInvalidFieldError("email", Phrases.InvalidEmailFormat);

            if (string.IsNullOrEmpty(model.Password))
                return ResponseModel<AuthenticateResponseModel>.CreateInvalidFieldError("passowrd", Phrases.FieldShouldNotBeEmpty);

            var client = await _clientAccountsRepository.AuthenticateAsync(model.Email, model.Password);

            if (client == null)
                return ResponseModel<AuthenticateResponseModel>.CreateInvalidFieldError("passowrd", Phrases.InvalidUsernameOrPassword);

            this.AuthenticateUserViaOwin(client);

            return ResponseModel<AuthenticateResponseModel>.CreateOk(new AuthenticateResponseModel
            {
                KycStatus = (await _kycRepository.GetKycStatusAsync(client.Id)).ToString(),
                PinIsEntered = await _pinSecurityRepository.IsPinEntered(client.Id)
            });
        }

    }

}
