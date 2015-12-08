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

        public Task<bool> Get(string pin)
        {
            var clientId = this.GetClientId();
            return _puPinSecurityRepository.CheckAsync(clientId, pin);
        }

        public async Task<object> Post(string pin)
        {

            if (string.IsNullOrEmpty(pin))
                return FailFieldModel.Create("pin", Phrases.FieldShouldNotBeEmpty);

            if (!pin.IsOnlyDigits())
                return FailFieldModel.Create("pin", Phrases.PinShouldContainsDigitsOnly);

            if (pin.Length<4)
                return FailFieldModel.Create("pin", Phrases.MinLengthIs4Digits);

            var clientId = this.GetClientId();

            await _puPinSecurityRepository.SaveAsync(clientId, pin);

            return OkResponseModel.Instance;
        }

    }
}
