using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Core.Assets;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Assets
{
    public class AssetEntity : TableEntity, IAsset
    {
        public static string GeneratePartitionKey()
        {
            return "Asset";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string Name { get; set; }


        public static AssetEntity Create(IAsset asset)
        {
            return new AssetEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(asset.Id),
                Name = asset.Name
            };
        }
    }

    public class AssetsRepository : IAssetsRepository
    {
        private readonly INoSQLTableStorage<AssetEntity> _tableStorage;

        public AssetsRepository(INoSQLTableStorage<AssetEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task RegisterAssetAsync(IAsset asset)
        {
            var newAsset = AssetEntity.Create(asset);
            return _tableStorage.InsertAsync(newAsset);
        }

        public async Task<IEnumerable<IAsset>> GetAssetsAsync()
        {
            var partitionKey = AssetEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);
        }
    }
}
