using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Kyc
{

    public interface IKycDocument
    {
        string ClientId { get; }
        string DocumentId { get;  }
        string Type { get; }
        string Mime { get; }

        string FileName { get; }
        DateTime DateTime { get; }
    }


    public class KycDocument : IKycDocument
    {
        public string ClientId { get; set; }
        public string DocumentId { get; set; }
        public string Type { get; set; }
        public string Mime { get; set; }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }

        public static KycDocument Create(string clientId, string type, string mime, string fileName)
        {
            return new KycDocument
            {
                ClientId = clientId,
                Type = type,
                Mime = mime,
                DateTime = DateTime.UtcNow,
                FileName = fileName
            };
        }
    }

    public interface IKycDocumentsRepository
    {
        Task<IKycDocument> AddAsync(IKycDocument kycDocument);
        Task<IEnumerable<IKycDocument>> GetAsync(string clientId);
        Task<IKycDocument> DeleteAsync(string clientId, string documentId);
    }


    public static class KycDocumentTypes
    {
        public const string IdCard = "IdCard";
        public const string ProofOfAddress = "ProofOfAddress";
        public const string Selfie = "Selfie";
    }

}
