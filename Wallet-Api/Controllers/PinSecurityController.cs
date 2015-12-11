﻿using System.Threading.Tasks;
using System.Web.Http;
using Common;
using Core.Clients;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    [Authorize]
    public class PinSecurityController : ApiController
    {
        private readonly IPinSecurityRepository _puPinSecurityRepository;

        public PinSecurityController(IPinSecurityRepository puPinSecurityRepository)
        {
            _puPinSecurityRepository = puPinSecurityRepository;
        }

        public async Task<ResponseModel<PinSecurityCheckResultModel>> Get(string pin)
        {
            var clientId = this.GetClientId();
            var passed = await _puPinSecurityRepository.CheckAsync(clientId, pin);
            return ResponseModel<PinSecurityCheckResultModel>.CreateOk(new PinSecurityCheckResultModel
            {
                Passed = passed
            });
        }

        public async Task<ResponseModel> Post(string pin)
        {

            if (string.IsNullOrEmpty(pin))
                return ResponseModel.CreateInvalidFieldError("pin", Phrases.FieldShouldNotBeEmpty);

            if (!pin.IsOnlyDigits())
                return ResponseModel.CreateInvalidFieldError("pin", Phrases.PinShouldContainsDigitsOnly);

            if (pin.Length<4)
                return ResponseModel.CreateInvalidFieldError("pin", Phrases.MinLengthIs4Digits);

            var clientId = this.GetClientId();

            await _puPinSecurityRepository.SaveAsync(clientId, pin);

            return ResponseModel.CreateOk();
        }

    }
}
