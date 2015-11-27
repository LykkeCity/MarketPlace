using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Common;
using Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace BackOfficeAzureRepositories.Users
{
    public class BackOfficeUserEntity : TableEntity, IBackOfficeUser
    {

        public static string GeneratePartitionKey()
        {
            return "BackOfficeUser";
        }

        public static string GenerateRowKey(string id)
        {
            return id.ToLower();
        }

        public string Id => RowKey;
        public string FullName { get; set; }
        public bool IsAdmin { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }


        public static BackOfficeUserEntity Create(IBackOfficeUser src, string password)
        {
            var result = new BackOfficeUserEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                IsAdmin = src.IsAdmin,
                FullName = src.FullName

            };

            result.SetPassword(password);

            return result;
        }

    }


    public static class BackOfficeUserExt
    {
        private static string CalcHash(string password, string salt)
        {

            var cryptoTransformSha1 = new SHA1CryptoServiceProvider();

            var sha1 = cryptoTransformSha1.ComputeHash((password + salt).ToUtf8Bytes());

            return Convert.ToBase64String(sha1);
        }

        public static void SetPassword(this BackOfficeUserEntity entity, string password)
        {
            entity.Salt = Guid.NewGuid().ToString();
            entity.Hash = CalcHash(password, entity.Salt);
        }

        public static bool CheckPassword(this BackOfficeUserEntity entity, string password)
        {
            var hash = CalcHash(password, entity.Salt);
            return entity.Hash == hash;
        }
    }


    public class BackOfficeUsersRepository : IBackOfficeUsersRepository
    {
        private readonly INoSQLTableStorage<BackOfficeUserEntity> _tableStorage;

        public BackOfficeUsersRepository(INoSQLTableStorage<BackOfficeUserEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveAsync(IBackOfficeUser backOfficeUser, string password)
        {
            var newUser = BackOfficeUserEntity.Create(backOfficeUser, password);
            return _tableStorage.InsertOrReplaceAsync(newUser);
        }

        public async Task<IBackOfficeUser> AuthenticateAsync(string username, string password)
        {
            if (username == null || password == null)
                return null;

            var partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            var rowKey = BackOfficeUserEntity.GenerateRowKey(username);


            var entity = await _tableStorage.GetDataAsync(partitionKey, rowKey);

            if (entity == null)
                return null;


            return entity.CheckPassword(password) ? entity : null;
        }

        public async Task<IBackOfficeUser> GetAsync(string id)
        {
            if (id == null)
                return null;

            var partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            var rowKey = BackOfficeUserEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<bool> UserExists(string id)
        {
            var partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            var rowKey = BackOfficeUserEntity.GenerateRowKey(id);

           return (await _tableStorage.GetDataAsync(partitionKey, rowKey)) != null;
        }

        public Task ChangePasswordAsync(string id, string newPassword)
        {
            var partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            var rowKey = BackOfficeUserEntity.GenerateRowKey(id);

            return _tableStorage.ReplaceAsync(partitionKey, rowKey, itm =>
            {
                itm.SetPassword(newPassword);
                return itm;
            });
        }

        public async Task<IEnumerable<IBackOfficeUser>> GetAllAsync()
        {
            var partitionKey = BackOfficeUserEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
