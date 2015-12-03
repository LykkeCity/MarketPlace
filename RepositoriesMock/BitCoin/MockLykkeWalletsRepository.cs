using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates;
using Core.BitCoin;

namespace RepositoriesMock.BitCoin
{
    public class MockLykkeAccountEntity : JsonTableEntity<LykkeAccount>
    {
        public static string GeneratePartitionKey()
        {
            return "LW";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public static MockLykkeAccountEntity Create(LykkeAccount src)
        {
            return new MockLykkeAccountEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                Instance = src
            };
        }

    }


    public class MockLykkeWalletsRepository : IMockLykkeWalletRepository
    {
        private readonly INoSQLTableStorage<MockLykkeAccountEntity> _tableStorage;

        public MockLykkeWalletsRepository(INoSQLTableStorage<MockLykkeAccountEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveAsync(LykkeAccount account)
        {
            var newEntity = MockLykkeAccountEntity.Create(account);
            return _tableStorage.InsertAsync(newEntity);
        }

        public async Task<LykkeAccount> GetAsync(string id)
        {
            var partitionKey = MockLykkeAccountEntity.GeneratePartitionKey();
            var rowKey = MockLykkeAccountEntity.GenerateRowKey(id);
            return (await _tableStorage.GetDataAsync(partitionKey, rowKey)).Instance;
        }

        public async Task<LykkeAccount> DepositWithdrawAsync(string id, double amount)
        {
            var partitionKey = MockLykkeAccountEntity.GeneratePartitionKey();
            var rowKey = MockLykkeAccountEntity.GenerateRowKey(id);
            var result = await _tableStorage.ReplaceAsync(partitionKey, rowKey, itm =>
            {
                var instance = itm.Instance;
                instance.Balance += amount;
                itm.Instance = instance;
                return itm;
            });

            return result.Instance;

        }
    }
}
