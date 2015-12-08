using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Common;
using Common.PasswordKeeping;
using Core.Clients;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Clients
{
    public class ClientAccountEntity : TableEntity, IClientAccount, IPasswordKeeping
    {
        public static string GeneratePartitionKey()
        {
            return "Trader";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public DateTime Registered { get; set; }
        public string Id => RowKey;
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Salt { get; set; }
        public string Hash { get; set; }

        public static ClientAccountEntity CreateNew(IClientAccount clientAccount, string password)
        {
            var result = new ClientAccountEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = Guid.NewGuid().ToString(),
                Email = clientAccount.Email.ToLower(),
                Phone = clientAccount.Phone,
                Registered = clientAccount.Registered
            };

            result.SetPassword(password);

            return result;
        }
    }


    public class ClientsRepository : IClientAccountsRepository
    {
        private readonly INoSQLTableStorage<ClientAccountEntity> _tradersTableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _emailIndices;

        private const string IndexEmail = "IndexEmail";

        public ClientsRepository(INoSQLTableStorage<ClientAccountEntity> tradersTableStorage, INoSQLTableStorage<AzureIndex> emailIndices)
        {
            _tradersTableStorage = tradersTableStorage;
            _emailIndices = emailIndices;
        }

        public async Task<IClientAccount> RegisterAsync(IClientAccount clientAccount, string password)
        {
            var newEntity = ClientAccountEntity.CreateNew(clientAccount, password);
            var indexEntity = AzureIndex.Create(IndexEmail, newEntity.Email, newEntity);

            await _emailIndices.InsertAsync(indexEntity);
            await _tradersTableStorage.InsertAsync(newEntity);

            return newEntity;
        }

        public async Task<bool> IsTraderWithEmailExistsAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return false;

            var indexEntity = await _emailIndices.GetDataAsync(IndexEmail, email);

            return indexEntity != null;
        }

        public async Task<IClientAccount> AuthenticateAsync(string email, string password)
        {
            if (email == null || password == null)
                return null;

            var indexEntity = await _emailIndices.GetDataAsync(IndexEmail, email);

            if (indexEntity == null)
                return null;

            var entity = await _tradersTableStorage.GetDataAsync(indexEntity);

            if (entity == null)
                return null;


            return entity.CheckPassword(password) ? entity : null;

        }

        public Task ChangePassword(string clientId, string newPassword)
        {
            var partitionKey = ClientAccountEntity.GeneratePartitionKey();
            var rowKey = ClientAccountEntity.GenerateRowKey(clientId);

            return _tradersTableStorage.ReplaceAsync(partitionKey, rowKey, itm =>
            {
                itm.SetPassword(newPassword);
                return itm;
            });
        }

        public async Task<IClientAccount> GetByIdAsync(string id)
        {
            var partitionKey = ClientAccountEntity.GeneratePartitionKey();
            var rowKey = ClientAccountEntity.GenerateRowKey(id);

            return await _tradersTableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IClientAccount> GetByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return null;

            return await _emailIndices.GetDataAsync(_tradersTableStorage, IndexEmail, email.ToLower());
        }
    }
}
