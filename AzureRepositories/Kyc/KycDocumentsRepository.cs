using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Core.Kyc;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Kyc
{
    public class KycDocumentEntity : TableEntity, IKycDocument
    {
        public static string GeneratePartitionKey(string clientId)
        {
            return clientId;
        }

        public static string GenerateRowKey(string documentId)
        {
            return documentId;
        }


        public string ClientId => PartitionKey;
        public string DocumentId => RowKey;
        public string Type { get; set; }
        public string Mime { get; set; }
        public string FileName { get; set; }
        public DateTime DateTime { get; set; }

        public static KycDocumentEntity Create(IKycDocument src)
        {
            return new KycDocumentEntity
            {
                PartitionKey = GeneratePartitionKey(src.ClientId),
                RowKey = GenerateRowKey(src.DocumentId ?? Guid.NewGuid().ToString()),
                Type = src.Type,
                Mime = src.Mime,
                DateTime = src.DateTime,
                FileName = src.FileName
            };
        }

    } 


    public class KycDocumentsRepository : IKycDocumentsRepository
    {
        private readonly INoSQLTableStorage<KycDocumentEntity> _tableStorage;

        public KycDocumentsRepository(INoSQLTableStorage<KycDocumentEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IKycDocument> AddAsync(IKycDocument kycDocument)
        {
            var newDocument = KycDocumentEntity.Create(kycDocument);
            await _tableStorage.InsertAsync(newDocument);
            return newDocument;
        }

        public async Task<IEnumerable<IKycDocument>> GetAsync(string clientId)
        {
            var partitionKey = KycDocumentEntity.GeneratePartitionKey(clientId);
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IKycDocument> DeleteAsync(string clientId, string documentId)
        {
            var partitionKey = KycDocumentEntity.GeneratePartitionKey(clientId);
            var rowKey = KycDocumentEntity.GenerateRowKey(documentId);
            return await _tableStorage.DeleteAsync(partitionKey, rowKey);
        }
    }
}
