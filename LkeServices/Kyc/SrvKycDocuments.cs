using System.Threading.Tasks;
using Core.Kyc;

namespace LkeServices.Kyc
{
    public class SrvKycDocuments
    {
        private readonly IKycDocumentsRepository _kycDocumentsRepository;
        private readonly IKycDocumentsScansRepository _kycDocumentsScansRepository;
        private readonly IKycUploadsLog _kycUploadsLog;
        private readonly IKycRepository _kycRepository;

        public SrvKycDocuments(IKycDocumentsRepository kycDocumentsRepository, IKycDocumentsScansRepository kycDocumentsScansRepository, 
            IKycUploadsLog kycUploadsLog, IKycRepository kycRepository)
        {
            _kycDocumentsRepository = kycDocumentsRepository;
            _kycDocumentsScansRepository = kycDocumentsScansRepository;
            _kycUploadsLog = kycUploadsLog;
            _kycRepository = kycRepository;
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


        public async Task ChangeKycStatus(string clientId, KycStatus kycStatus)
        {
            await _kycRepository.SetStatusAsync(clientId, kycStatus);
        }


    }

}
