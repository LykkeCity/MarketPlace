using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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

        public KycDocumentsController(SrvKycDocumentsManager srvKycDocumentsManager)
        {
            _srvKycDocumentsManager = srvKycDocumentsManager;
        }

        private static readonly Dictionary<string, string> DocumentTypes = new Dictionary<string, string>
        {
            {"id", KycDocumentTypes.IdCard},
            {"poa", KycDocumentTypes.ProofOfAddress},
            {"selfie", KycDocumentTypes.Selfie},


        }; 

        public async Task<ResponseModel> Post(string type, string ext)
        {
            if (string.IsNullOrEmpty(type))
                return ResponseModel.CreateInvalidFieldError("type", Phrases.FieldShouldNotBeEmpty);

            if(!DocumentTypes.ContainsKey(type))
                return ResponseModel.CreateInvalidFieldError("type", Phrases.InvalidDocumentType);

            if (string.IsNullOrEmpty(ext))
                return ResponseModel.CreateInvalidFieldError("ext", Phrases.FieldShouldNotBeEmpty);

            var stream = new MemoryStream();
            await Request.Content.CopyToAsync(stream);

            var clientId = this.GetClientId();

            var fileName = "myFile"+ext.AddFirstSymbolIfNotExists('.');
            var mimeType = MimeMapping.GetMimeMapping(fileName);

            await
                _srvKycDocumentsManager.UploadDocument(clientId, DocumentTypes[type], fileName, mimeType,
                    stream.ToArray());

            return ResponseModel.CreateOk();
        }

    }
}
