using System.Threading.Tasks;
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
            if (string.IsNullOrEmpty(clientId))
                return ResponseModel<PinSecurityCheckResultModel>.CreateFail(ResponseModel.ErrorCodeType.NotAuthenticated, Phrases.NotAuthenticated);

            var passed = await _puPinSecurityRepository.CheckAsync(clientId, pin);
            return ResponseModel<PinSecurityCheckResultModel>.CreateOk(new PinSecurityCheckResultModel
            {
                Passed = passed
            });

        }

        public async Task<ResponseModel> Post(PinSecurityChangeModel data)
        {

            if (string.IsNullOrEmpty(data.Pin))
                return ResponseModel.CreateInvalidFieldError("pin", Phrases.FieldShouldNotBeEmpty);

            if (!data.Pin.IsOnlyDigits())
                return ResponseModel.CreateInvalidFieldError("pin", Phrases.PinShouldContainsDigitsOnly);

            if (data.Pin.Length<4)
                return ResponseModel.CreateInvalidFieldError("pin", Phrases.MinLengthIs4Digits);

            var clientId = this.GetClientId();
            if (string.IsNullOrEmpty(clientId))
                return ResponseModel<PinSecurityCheckResultModel>.CreateFail(ResponseModel.ErrorCodeType.NotAuthenticated, Phrases.NotAuthenticated);
            await _puPinSecurityRepository.SaveAsync(clientId, data.Pin);

            return ResponseModel.CreateOk();
        }

    }
}
