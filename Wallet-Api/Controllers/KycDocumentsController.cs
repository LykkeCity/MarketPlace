using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Common;
using Core.Clients;
using Core.Kyc;
using LkeServices.Kyc;
using Wallet_Api.Models;
using Wallet_Api.Strings;

namespace Wallet_Api.Controllers
{
    [Authorize]
    public class KycDocumentsController : ApiController
    {
        private readonly SrvKycDocumentsManager _srvKycDocumentsManager;
        private readonly IPersonalDataRepository _personalDataRepository;
        private readonly IKycRepository _kycRepository;


        public KycDocumentsController(IKycRepository kycRepository, SrvKycDocumentsManager srvKycDocumentsManager, IPersonalDataRepository personalDataRepository)
        {
            _srvKycDocumentsManager = srvKycDocumentsManager;
            _personalDataRepository = personalDataRepository;
            _kycRepository = kycRepository;
        }


        public async Task<ResponseModel<PostKycDocumentRespModel>> Post(KycDocumentsModel model)
        {

            if (string.IsNullOrEmpty(model.Type))
                return ResponseModel<PostKycDocumentRespModel>.CreateInvalidFieldError("type", Phrases.FieldShouldNotBeEmpty);


            if(!KycDocumentTypes.HasDocumentType(model.Type))
                return ResponseModel<PostKycDocumentRespModel>.CreateInvalidFieldError("type", Phrases.InvalidDocumentType);

            if (string.IsNullOrEmpty(model.Ext))
                return ResponseModel<PostKycDocumentRespModel>.CreateInvalidFieldError("ext", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(model.Data))
                return ResponseModel<PostKycDocumentRespModel>.CreateInvalidFieldError("data", Phrases.FieldShouldNotBeEmpty);


            byte[] data;
            try
            {
                data = Convert.FromBase64String(model.Data);
            }
            catch (Exception)
            {

                return ResponseModel<PostKycDocumentRespModel>.CreateInvalidFieldError("data", "Base64 format expected");
            }

            var clientId = this.GetClientId();

            if (string.IsNullOrEmpty(clientId))
                return ResponseModel<PostKycDocumentRespModel>.CreateFail(ResponseModel.ErrorCodeType.NotAuthenticated, Phrases.NotAuthenticated);

            var status = await _kycRepository.GetKycStatusAsync(clientId);
            if (status != KycStatus.NeedToFillData)
                return ResponseModel<PostKycDocumentRespModel>.CreateFail(ResponseModel.ErrorCodeType.InconsistentData, Phrases.OperationCanNotBePerformed);


            var fileName = "myFile"+ model.Ext.AddFirstSymbolIfNotExists('.');
            var mimeType = MimeMapping.GetMimeMapping(fileName);

           await
              _srvKycDocumentsManager.UploadDocument(clientId, model.Type, fileName, mimeType, data);


            var personalData = await _personalDataRepository.GetAsync(clientId);
            return ResponseModel<PostKycDocumentRespModel>.CreateOk(PostKycDocumentRespModel.Create(personalData.ConvertToApiModel()));
        }

    }
}
