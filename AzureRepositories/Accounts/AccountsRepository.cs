using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Core.Accounts;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Accounts
{
    public class AccountEntity : TableEntity, IAccount
    {
        public static string GeneratePartitionKey(string clientId)
        {
            return clientId;
        }

        public static string GenerateRowKey(string accountId)
        {
            return accountId;
        }

        public string ClientId => PartitionKey;
        public string AccountId => RowKey;

        public double Balance { get; set; }
        public string CurrencyId { get; set; }

        public static AccountEntity Create(IAccount src)
        {
            return new AccountEntity
            {
                PartitionKey = GeneratePartitionKey(src.ClientId),
                RowKey = GenerateRowKey(src.AccountId),
                Balance = src.Balance,
                CurrencyId = src.CurrencyId
            };
        }
    }

    public class AccountsRepository : IAccountsRepository
    {
        private readonly INoSQLTableStorage<AccountEntity> _tableStorage;

        public AccountsRepository(INoSQLTableStorage<AccountEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }


        public Task RegisterAccount(IAccount src)
        {
            var newEntity = AccountEntity.Create(src);
            return _tableStorage.InsertAsync(newEntity);
        }

        public async Task<IEnumerable<IAccount>> GetAccountsAsync(string clientId)
        {
            var partitionKey = AccountEntity.GeneratePartitionKey(clientId);
            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
