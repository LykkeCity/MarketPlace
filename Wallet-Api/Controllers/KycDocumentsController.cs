using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Common;
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
        private readonly IKycRepository _kycRepository;


        public KycDocumentsController(IKycRepository kycRepository, SrvKycDocumentsManager srvKycDocumentsManager)
        {
            _srvKycDocumentsManager = srvKycDocumentsManager;
            _kycRepository = kycRepository;
        }


        public async Task<ResponseModel> Post(KycDocumentsModel model)
        {

            if (string.IsNullOrEmpty(model.Type))
                return ResponseModel.CreateInvalidFieldError("type", Phrases.FieldShouldNotBeEmpty);


            if(!KycDocumentTypes.HasDocumentType(model.Type))
                return ResponseModel.CreateInvalidFieldError("type", Phrases.InvalidDocumentType);

            if (string.IsNullOrEmpty(model.Ext))
                return ResponseModel.CreateInvalidFieldError("ext", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(model.Data))
                return ResponseModel.CreateInvalidFieldError("data", Phrases.FieldShouldNotBeEmpty);


            byte[] data;
            try
            {
                data = Convert.FromBase64String(model.Data);
            }
            catch (Exception)
            {

                return ResponseModel.CreateInvalidFieldError("data", "Base64 format expected");
            }

            var clientId = this.GetClientId();

            if (string.IsNullOrEmpty(clientId))
                return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.NotAuthenticated, Phrases.NotAuthenticated);

            var status = await _kycRepository.GetKycStatusAsync(clientId);
            if (status != KycStatus.NeedToFillData)
                return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.InconsistentData, Phrases.OperationCanNotBePerformed);


            var fileName = "myFile"+ model.Ext.AddFirstSymbolIfNotExists('.');
            var mimeType = MimeMapping.GetMimeMapping(fileName);

           await
              _srvKycDocumentsManager.UploadDocument(clientId, model.Type, fileName, mimeType, data);

            return ResponseModel.CreateOk();
        }

    }
}
