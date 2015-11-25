using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Core.Finance;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Finance
{

    public class TraderBalanceEntity : TableEntity, ITraderBalance
    {
        public static string GeneratePartitionKey(string traderId)
        {
            return traderId;
        }

        public static string GenerateRowKey(string currency)
        {
            return currency.ToUpper();
        }

        public string TraderId => PartitionKey;
        public string Currency => RowKey;
        public double Amount { get; set; }


        public static TraderBalanceEntity Create(string traderId, string currency)
        {
            return new TraderBalanceEntity
            {
                PartitionKey = GeneratePartitionKey(traderId),
                RowKey = GenerateRowKey(currency)
            };
        }
    }

    public class BalanceRepository : IBalanceRepository
    {
        private readonly INoSQLTableStorage<TraderBalanceEntity> _tableStorage;

        public BalanceRepository(INoSQLTableStorage<TraderBalanceEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<ITraderBalance>> GetAsync(string traderId)
        {
            var partitionKey = TraderBalanceEntity.GeneratePartitionKey(traderId);
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task ChangeBalanceAsync(string traderId, string currency, double delta)
        {
            var partitionKey = TraderBalanceEntity.GeneratePartitionKey(traderId);
            var rowKey = TraderBalanceEntity.GenerateRowKey(currency);

            var entity = await _tableStorage.ReplaceAsync(partitionKey, rowKey, itm =>
            {
                itm.Amount += delta;
                return itm;
            });


            if (entity != null)
                return;

            entity = TraderBalanceEntity.Create(traderId, currency);
            entity.Amount = delta;

            await _tableStorage.InsertAsync(entity);

        }
    }
}
