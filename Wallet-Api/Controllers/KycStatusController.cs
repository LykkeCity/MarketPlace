using System.Threading.Tasks;
using System.Web.Http;
using Core.Kyc;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    [Authorize]
    public class KycStatusController : ApiController
    {
        private readonly IKycRepository _kycRepository;

        public KycStatusController(IKycRepository kycRepository)
        {
            _kycRepository = kycRepository;
        }

        public async Task<ResponseModel<KycModelStatusResponseModel>> Get()
        {
            var clientId = this.GetClientId();
            if (string.IsNullOrEmpty(clientId))
                return ResponseModel<KycModelStatusResponseModel>.CreateFail(ResponseModel.ErrorCodeType.NotAuthenticated, Phrases.NotAuthenticated);

            var kycStatus = await _kycRepository.GetKycStatusAsync(clientId);


            
            return ResponseModel<KycModelStatusResponseModel>.CreateOk(
                new KycModelStatusResponseModel
                {
                    KycStatus = kycStatus.ToResponseModel()
                });
        }

        public async Task<ResponseModel> Post()
        {
            var clientId = this.GetClientId();
            if (string.IsNullOrEmpty(clientId))
                return ResponseModel<KycModelStatusResponseModel>.CreateFail(ResponseModel.ErrorCodeType.NotAuthenticated, Phrases.OperationCanNotBePerformed);

            var status = await _kycRepository.GetKycStatusAsync(clientId);

            if (status == KycStatus.NeedToFillData)
            {
                await _kycRepository.SetStatusAsync(clientId, KycStatus.Pending);
                return ResponseModel.CreateOk();
            }

            if (status == KycStatus.Pending)
                return ResponseModel.CreateOk();

            return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.InconsistentData, Phrases.OperationCanNotBePerformed);
        }

    }
}
