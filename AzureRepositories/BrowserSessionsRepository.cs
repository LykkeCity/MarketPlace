using System.Threading.Tasks;
using Common;
using Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories
{
    public class BrowserSessionEntity : TableEntity, IBrowserSession
    {

        public static string GeneratePartitionKey()
        {
            return "BrowSess";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string UserName { get; set; }


        public static BrowserSessionEntity Create(string id, string userName)
        {
            return new BrowserSessionEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(id),
                UserName = userName
            };
        }
    }

    public class BrowserSessionsRepository : IBrowserSessionsRepository
    {
        private readonly INoSQLTableStorage<BrowserSessionEntity> _tableStorage;

        public BrowserSessionsRepository(INoSQLTableStorage<BrowserSessionEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IBrowserSession> GetSessionAsync(string sessionId)
        {
            var partitionKey = BrowserSessionEntity.GeneratePartitionKey();
            var rowKey = BrowserSessionEntity.GenerateRowKey(sessionId);
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public Task SaveSessionAsync(string sessionId, string userId)
        {
            var newEntity = BrowserSessionEntity.Create(sessionId, userId);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }
    }
}
