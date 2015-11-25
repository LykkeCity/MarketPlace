using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Core.Assets;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Assets
{

    public class AssetPairEntity : TableEntity, IAssetPair
    {
        public static string GeneratePartitionKey()
        {
            return "AssetPair";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;

        public string BaseAssetId { get; set; }
        public string QuotingAssetId { get; set; }
        public int Accuracy { get; set; }


        public static AssetPairEntity Create(IAssetPair src)
        {
            return new AssetPairEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                Accuracy = src.Accuracy,
                BaseAssetId = src.BaseAssetId,
                QuotingAssetId = src.QuotingAssetId
            };
        }
    }

    public class AssetPairsRepository : IAssetPairsRepository
    {
        private readonly INoSQLTableStorage<AssetPairEntity> _tableStorage;

        public AssetPairsRepository(INoSQLTableStorage<AssetPairEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IAssetPair>> GetAllAsync()
        {
            var partitionKey = AssetPairEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public Task SaveAsync(IAssetPair assetPair)
        {
            var newEntity = AssetPairEntity.Create(assetPair);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }
    }
}
