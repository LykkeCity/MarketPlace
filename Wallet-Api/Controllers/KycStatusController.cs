using System.Threading.Tasks;
using System.Web.Http;
using Core.Kyc;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    public class KycStatusController : ApiController
    {
        private readonly IKycRepository _kycRepository;

        public KycStatusController(IKycRepository kycRepository)
        {
            _kycRepository = kycRepository;
        }

        public async Task<object> Get()
        {
            var clientId = this.GetClientId();
            return (await _kycRepository.GetKycStatusAsync(clientId)).ToString();
        }

        public async Task<object> Post()
        {
            var clientId = this.GetClientId();

            var status = await _kycRepository.GetKycStatusAsync(clientId);

            if (status == KycStatus.NeedToFillData)
            {
                await _kycRepository.SetStatusAsync(clientId, KycStatus.Pending);
                return OkResponseModel.Instance;
            }

            if (status == KycStatus.Pending)
                return OkResponseModel.Instance;

            return FailResponseModel.Create(Phrases.OperationCanNotBePerformed);
        }

    }
}
