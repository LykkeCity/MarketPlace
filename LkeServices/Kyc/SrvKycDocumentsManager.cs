using System.Threading.Tasks;
using Core.Kyc;

namespace LkeServices.Kyc
{
    public class SrvKycDocumentsManager
    {
        private readonly IKycDocumentsRepository _kycDocumentsRepository;
        private readonly IKycDocumentsScansRepository _kycDocumentsScansRepository;
        private readonly IKycUploadsLog _kycUploadsLog;

        public SrvKycDocumentsManager(IKycDocumentsRepository kycDocumentsRepository, IKycDocumentsScansRepository kycDocumentsScansRepository, 
            IKycUploadsLog kycUploadsLog)
        {
            _kycDocumentsRepository = kycDocumentsRepository;
            _kycDocumentsScansRepository = kycDocumentsScansRepository;
            _kycUploadsLog = kycUploadsLog;
        }

        public async Task<string> UploadDocument(string clientId, string type, string fileName, string mime, byte[] data)
        {
            var kycDocument = await _kycDocumentsRepository.AddAsync(KycDocument.Create(clientId, type, mime, fileName));
            await _kycDocumentsScansRepository.AddDocument(kycDocument.DocumentId, data);
            await _kycUploadsLog.AddAsync(KycUploadsLogItem.Create(clientId, type, kycDocument.DocumentId, mime));
            return kycDocument.DocumentId;
        }

        public async Task<IKycDocument> DeleteAsync(string clientId, string documentId)
        {
            return await _kycDocumentsRepository.DeleteAsync(clientId, documentId);
        }

    }

}
