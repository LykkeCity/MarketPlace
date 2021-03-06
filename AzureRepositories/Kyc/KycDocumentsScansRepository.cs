﻿using System.Threading.Tasks;
using AzureStorage;
using Common;
using Core.Kyc;

namespace AzureRepositories.Kyc
{

    public class KycDocumentsScansRepository : IKycDocumentsScansRepository
    {
        private readonly IBlobStorage _blobStorage;
        private const string ContainerName = "documents";
        public KycDocumentsScansRepository(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }

        public Task AddDocument(string id, byte[] data)
        {
            return _blobStorage.SaveBlobAsync(ContainerName, id, data);
        }

        public async Task<byte[]> GetDocument(string id)
        {
            var stream = await _blobStorage.GetAsync(ContainerName, id);
            return await stream.ToBytesAsync();
        }
    }
}
