﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Core.Kyc;
using Wallet_Api.Models;

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


        public async Task<ResponseModel<CheckDocumentsToUploadModel>> Get()
        {
            var clientId = this.GetClientId();
            var documents = (await _kycDocumentsRepository.GetAsync(clientId)).ToArray();


            var result = new CheckDocumentsToUploadModel
            {
                IdCard = documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.IdCard) == null,
                ProofOfAddress = documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.ProofOfAddress) == null,
                Selfie = documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.Selfie) == null
            };

            return ResponseModel<CheckDocumentsToUploadModel>.CreateOk(result);
        }
    }
}
