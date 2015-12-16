using System.Threading.Tasks;
using System.Web.Http;
using Core.Clients;
using Wallet_Api.Models;

namespace Wallet_Api.Controllers
{

    [ApiAuthenticationHandler]
    public class IsPinEnteredController : ApiController
    {
        private readonly IPinSecurityRepository _pinSecurityRepository;

        public IsPinEnteredController(IPinSecurityRepository pinSecurityRepository)
        {
            _pinSecurityRepository = pinSecurityRepository;
        }

        public async Task<ResponseModel<IsPinSecurityEnabledResultModel>> Get()
        {
            var clientId = this.GetClientId();

            var isEnabled = await _pinSecurityRepository.IsPinEntered(clientId);

            return
                ResponseModel<IsPinSecurityEnabledResultModel>.CreateOk(new IsPinSecurityEnabledResultModel
                {
                    IsEnabled = isEnabled
                });
        }
    }

}
