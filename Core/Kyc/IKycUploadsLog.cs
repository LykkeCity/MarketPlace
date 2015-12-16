using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Kyc
{
    public interface IKycUploadsLogItem
    {
        DateTime DateTime { get; }
        string ClientId { get; }
        string Type { get; }
        string DocumentId { get; }
        string Mime { get; }
    }

    public class KycUploadsLogItem : IKycUploadsLogItem
    {
        public DateTime DateTime { get; set; }
        public string ClientId { get; set; }
        public string Type { get; set; }
        public string DocumentId { get; set; }
        public string Mime { get; set; }

        public static KycUploadsLogItem Create(string clientId, string type, string documentId, string mime)
        {
            return new KycUploadsLogItem
            {
                DateTime = DateTime.UtcNow,
                Type = type,
                Mime = mime,
                ClientId = clientId,
                DocumentId = documentId
            };
        }
    }

    public interface IKycUploadsLog
    {
        Task AddAsync(IKycUploadsLogItem itm);

    }
}
