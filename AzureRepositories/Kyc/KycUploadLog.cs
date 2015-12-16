using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureStorage;
using Common;
using Core.Kyc;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Kyc
{
    public class KycUploadsLogItemEntity : TableEntity, IKycUploadsLogItem
    {
        public static string GeneratePartitionKey(string clientId)
        {
            return clientId;
        }

        public string ClientId => PartitionKey;
        public DateTime DateTime { get; set; }
        
        public string Type { get; set; }
        public string DocumentId { get; set; }
        public string Mime { get; set; }

        public static KycUploadsLogItemEntity Create(IKycUploadsLogItem src)
        {
            return new KycUploadsLogItemEntity
            {
                PartitionKey = GeneratePartitionKey(src.ClientId),
                DateTime = src.DateTime,
                DocumentId = src.DocumentId,
                Mime = src.Mime,
                Type = src.Type
            };
        }



    }

    public class KycUploadsLog : IKycUploadsLog
    {
        private readonly INoSQLTableStorage<KycUploadsLogItemEntity> _tableStorage;

        public KycUploadsLog(INoSQLTableStorage<KycUploadsLogItemEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task AddAsync(IKycUploadsLogItem itm)
        {
            var newEntity = KycUploadsLogItemEntity.Create(itm);
            return _tableStorage.InsertAndGenerateRowKeyAsDateTimeAsync(newEntity, newEntity.DateTime);

        }
    }
}
