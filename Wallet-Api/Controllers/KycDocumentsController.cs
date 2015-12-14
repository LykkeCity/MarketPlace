using System.IO;
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
    /// <summary>
    /// Client Documents upload link
    /// </summary>
    [Authorize]
    public class KycDocumentsController : ApiController
    {
        private readonly SrvKycDocumentsManager _srvKycDocumentsManager;
        private readonly IKycRepository _kycRepository;

        /// <summary>
        /// Client Documents upload link
        /// </summary>
        public KycDocumentsController(SrvKycDocumentsManager srvKycDocumentsManager, IKycRepository kycRepository)
        {
            _srvKycDocumentsManager = srvKycDocumentsManager;
            _kycRepository = kycRepository;
        }

        /// <summary>
        /// Upload client document. Operation can be perfomed if client KYC status is NeedToFeelData only
        /// </summary>
        /// <param name="type">Document Type; IdCard/ProofOfAddress/Selfie</param>
        /// <param name="ext"></param>
        /// <returns></returns>
        public async Task<ResponseModel> Post(string type, string ext)
        {

            if (string.IsNullOrEmpty(type))
                return ResponseModel.CreateInvalidFieldError("type", Phrases.FieldShouldNotBeEmpty);


            if(!KycDocumentTypes.HasDocumentType(type))
                return ResponseModel.CreateInvalidFieldError("type", Phrases.InvalidDocumentType);

            if (string.IsNullOrEmpty(ext))
                return ResponseModel.CreateInvalidFieldError("ext", Phrases.FieldShouldNotBeEmpty);

            var clientId = this.GetClientId();

            var status = await _kycRepository.GetKycStatusAsync(clientId);
            if (status != KycStatus.NeedToFillData)
                return ResponseModel.CreateFail(ResponseModel.ErrorCodeType.InconsistentData, Phrases.OperationCanNotBePerformed);

            var stream = new MemoryStream();
            await Request.Content.CopyToAsync(stream);

            var fileName = "myFile"+ext.AddFirstSymbolIfNotExists('.');
            var mimeType = MimeMapping.GetMimeMapping(fileName);

            await
                _srvKycDocumentsManager.UploadDocument(clientId, type, fileName, mimeType,
                    stream.ToArray());

            return ResponseModel.CreateOk();
        }

    }
}
