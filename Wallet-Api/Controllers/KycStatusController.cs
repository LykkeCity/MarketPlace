using System.Threading.Tasks;
using System.Web.Http;
using Core.Clients;
using Core.Kyc;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    [Authorize]
    public class KycStatusController : ApiController
    {
        private readonly IKycRepository _kycRepository;
        private readonly IPersonalDataRepository _personalDataRepository;

        public KycStatusController(IKycRepository kycRepository, IPersonalDataRepository personalDataRepository)
        {
            _kycRepository = kycRepository;
            _personalDataRepository = personalDataRepository;
        }

        public async Task<ResponseModel<GetKycStatusRespModel>> Get()
        {
            var clientId = this.GetClientId();
            if (string.IsNullOrEmpty(clientId))
                return ResponseModel<GetKycStatusRespModel>.CreateFail(ResponseModel.ErrorCodeType.NotAuthenticated, Phrases.NotAuthenticated);

            var kycStatus = await _kycRepository.GetKycStatusAsync(clientId);
            var personalData = kycStatus != KycStatus.Pending ? await _personalDataRepository.GetAsync(clientId) : null;

            return
                ResponseModel<GetKycStatusRespModel>.CreateOk(GetKycStatusRespModel.Create(kycStatus.ToResponseModel(),
                    personalData.ConvertToApiModel()));

        }

        public async Task<ResponseModel<PostKycStatusRespModel>> Post()
        {
            var clientId = this.GetClientId();
            if (string.IsNullOrEmpty(clientId))
                return ResponseModel<PostKycStatusRespModel>.CreateFail(ResponseModel.ErrorCodeType.NotAuthenticated, Phrases.OperationCanNotBePerformed);

            var status = await _kycRepository.GetKycStatusAsync(clientId);
            var personalData =  await _personalDataRepository.GetAsync(clientId);

            if (status == KycStatus.NeedToFillData)
            {
                await _kycRepository.SetStatusAsync(clientId, KycStatus.Pending);
                return ResponseModel<PostKycStatusRespModel>.CreateOk(PostKycStatusRespModel.Create(personalData.ConvertToApiModel()));
            }

            if (status == KycStatus.Pending)
                return ResponseModel<PostKycStatusRespModel>.CreateOk(PostKycStatusRespModel.Create(personalData.ConvertToApiModel()));

            return ResponseModel<PostKycStatusRespModel>.CreateFail(ResponseModel.ErrorCodeType.InconsistentData, Phrases.OperationCanNotBePerformed);
        }

    }
}
