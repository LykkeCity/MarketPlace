using System.Collections.Generic;
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

        public async Task<IAssetPair> GetAsync(string id)
        {
            var partitionKey = AssetPairEntity.GeneratePartitionKey();
            var rowKey = AssetPairEntity.GenerateRowKey(id);
            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public Task AddAsync(IAssetPair assetPair)
        {
            var newEntity = AssetPairEntity.Create(assetPair);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }

        public async Task EditAsync(string id, IAssetPair assetPair)
        {
            await _tableStorage.DeleteAsync(AssetPairEntity.GeneratePartitionKey(), AssetPairEntity.GenerateRowKey(id));
            await AddAsync(assetPair);
        }

    }
}
