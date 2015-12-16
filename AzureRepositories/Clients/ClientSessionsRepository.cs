using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Core.Clients;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Clients
{
    public class ClientSessionEntity : TableEntity, IClientSession
    {
        public static class ByToken
        {
            public static string GeneratePartitionKey()
            {
                return "Sess";
            }

            public static string GenerateRowKey(string sessionId)
            {
                return sessionId;
            }
        }

        public static class ByClient
        {
            public static string GeneratePartitionKey(string clientId)
            {
                return clientId;
            }

            public static string GenerateRowKey(string sessionId)
            {
                return sessionId;
            }
        }

        public string ClientId { get; set; }
        public string Token => RowKey;
        public DateTime Registered { get; set; }
        public DateTime LastAction { get; set; }

        public static ClientSessionEntity CreateByToken(string clientId, string token)
        {
            return new ClientSessionEntity
            {
                PartitionKey = ByToken.GeneratePartitionKey(),
                RowKey = ByToken.GenerateRowKey(token),
                ClientId = clientId,
                Registered = DateTime.UtcNow,
                LastAction = DateTime.UtcNow
            };
        }

        public static ClientSessionEntity CreateByClient(string clientId, string token)
        {
            return new ClientSessionEntity
            {
                PartitionKey = ByClient.GeneratePartitionKey(clientId),
                RowKey = ByClient.GenerateRowKey(token),
                ClientId = clientId,
                Registered = DateTime.UtcNow,
                LastAction = DateTime.UtcNow
            };
        }

    }


    public class ClientSessionsRepository : IClientsSessionsRepository
    {
        private readonly INoSQLTableStorage<ClientSessionEntity> _tableStorage;

        public ClientSessionsRepository(INoSQLTableStorage<ClientSessionEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveAsync(string clientId, string sessionId)
        {
            return Task.WhenAll(
                _tableStorage.InsertAsync(ClientSessionEntity.CreateByToken(clientId, sessionId)),
                _tableStorage.InsertAsync(ClientSessionEntity.CreateByClient(clientId, sessionId))
                );
        }

        public async Task<IClientSession> GetAsync(string sessionId)
        {
            var partitionKey = ClientSessionEntity.ByToken.GeneratePartitionKey();
            var rowKey = ClientSessionEntity.ByToken.GenerateRowKey(sessionId);
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IClientSession>> GetByClientAsync(string clientId)
        {
            var partitionKey = ClientSessionEntity.ByClient.GeneratePartitionKey(clientId);
            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
