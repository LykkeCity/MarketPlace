using System.Threading.Tasks;
using AzureStorage;
using Common;
using Core;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories
{

    public class IdentityEntity : TableEntity
    {
        public const string GeneratePartitionKey = "Id";
        public const string GenerateRowKey = "Id";

        public int Value { get; set; }


        public static IdentityEntity Create()
        {
            return new IdentityEntity
            {
                PartitionKey = GeneratePartitionKey,
                RowKey = GenerateRowKey,
                Value = 0
            };
        }
    }


    public class IdentityGenerator : IIdentityGenerator
    {
        private readonly INoSQLTableStorage<IdentityEntity> _tableStorage;

        public IdentityGenerator(INoSQLTableStorage<IdentityEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<int> GenerateNewIdAsync()
        {
            var entity =
                await
                    _tableStorage.InsertOrModifyAsync(IdentityEntity.GeneratePartitionKey, IdentityEntity.GenerateRowKey,
                        IdentityEntity.Create,
                        itm =>
                        {
                            itm.Value++;
                            return itm;
                        }
                        );


            return entity.Value;
        }
    }
}
