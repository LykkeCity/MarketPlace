using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Kyc;

namespace Wallet_Api.Controllers
{
    [Authorize]
    public class CheckDocumentsToUploadController : ApiController
    {
        private readonly IKycDocumentsRepository _kycDocumentsRepository;

        public CheckDocumentsToUploadController(IKycDocumentsRepository kycDocumentsRepository)
        {
            _kycDocumentsRepository = kycDocumentsRepository;
        }


        public async Task<object> Get()
        {
            var clientId = this.GetClientId();
            var documents = (await _kycDocumentsRepository.GetAsync(clientId)).ToArray();

            return new
            {
                id = documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.IdCard) == null,
                poa = documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.ProofOfAddress) == null,
                selfie = documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.Selfie) == null
            };
        }
    }
}
