﻿using System.Threading.Tasks;
using AzureStorage;
using Core.Clients;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Clients
{
    public class ClientSettingsEntity : TableEntity
    {
        public static string GeneratePartitionKey(string traderId)
        {
            return traderId;
        }

        public static string GenerateRowKey(TraderSettingsBase settingsBase)
        {
            return settingsBase.GetId();
        }

        public string Data { get; set; }

        internal T GetSettings<T>() where T : TraderSettingsBase
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Data);
        }

        internal void SetSettings(TraderSettingsBase settings)
        {
            Data = Newtonsoft.Json.JsonConvert.SerializeObject(settings);
        }


        public static ClientSettingsEntity Create(string traderId, TraderSettingsBase settings)
        {
            var result = new ClientSettingsEntity
            {
                PartitionKey = GeneratePartitionKey(traderId),
                RowKey = GenerateRowKey(settings),
            };
            result.SetSettings(settings);
            return result;
        }
    }


    public class ClientSettingsRepository : IClientSettingsRepository
    {
        private readonly INoSQLTableStorage<ClientSettingsEntity> _tableStorage;

        public ClientSettingsRepository(INoSQLTableStorage<ClientSettingsEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<T> GetSettings<T>(string traderId) where T : TraderSettingsBase, new()
        {
            var partitionKey = ClientSettingsEntity.GeneratePartitionKey(traderId);
            var defaultValue = TraderSettingsBase.CreateDefault<T>();
            var rowKey = ClientSettingsEntity.GenerateRowKey(defaultValue);
            var entity = await _tableStorage.GetDataAsync(partitionKey, rowKey);
            return entity == null ? defaultValue : entity.GetSettings<T>();
        }

        public Task SetSettings<T>(string traderId, T settings) where T : TraderSettingsBase, new()
        {
            var newEntity = ClientSettingsEntity.Create(traderId, settings);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }

    }
}
