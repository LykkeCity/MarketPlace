using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AzureStorage.Tables.Templates;
using Common;
using Core.Clients;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Traders
{
    public class ClientAccountEntity : TableEntity, IClientAccount
    {
        public static string GeneratePartitionKey()
        {
            return "Trader";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

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
                Phone = clientAccount.Phone
            };

            result.SetPassword(password);

            return result;
        }
    }

    public static class TraderEntityExt
    {
        private static string CalcHash(string password, string salt)
        {

            var cryptoTransformSha1 = new SHA1CryptoServiceProvider();

            var sha1 = cryptoTransformSha1.ComputeHash((password + salt).ToUtf8Bytes());

            return Convert.ToBase64String(sha1);
        }

        public static void SetPassword(this ClientAccountEntity entity, string password)
        {
            entity.Salt = Guid.NewGuid().ToString();
            entity.Hash = CalcHash(password, entity.Salt);
        }

        public static bool CheckPassword(this ClientAccountEntity entity, string password)
        {
            var hash = CalcHash(password, entity.Salt);
            return entity.Hash == hash;
        }

    }


    public class TradersRepository : IClientAccountsRepository
    {
        private readonly INoSQLTableStorage<ClientAccountEntity> _tradersTableStorage;
        private readonly INoSQLTableStorage<AzureIndex> _emailIndices;

        private const string IndexEmail = "IndexEmail";

        public TradersRepository(INoSQLTableStorage<ClientAccountEntity> tradersTableStorage, INoSQLTableStorage<AzureIndex> emailIndices)
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

        public async Task<IClientAccount> GetByIdAsync(string id)
        {
            var partitionKey = ClientAccountEntity.GeneratePartitionKey();
            var rowKey = ClientAccountEntity.GenerateRowKey(id);

            return await _tradersTableStorage.GetDataAsync(partitionKey, rowKey);
        }
    }
}
