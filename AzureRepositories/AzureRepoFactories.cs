using AzureRepositories.Assets;
using AzureRepositories.Finance;
using AzureRepositories.Orders;
using AzureRepositories.Traders;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates;
using Common.Log;

namespace AzureRepositories
{
    public static class AzureRepoFactories
    {

        public static TradersRepository CreateTradersRepository(string connstring, ILog log)
        {
            const string tableName = "Traders";
            return new TradersRepository(
                new AzureTableStorage<TraderEntity>(connstring, tableName, log), 
                new AzureTableStorage<AzureIndex>(connstring, tableName, log));
        }

        public static BalanceRepository CreateBalanceRepository(string connstring, ILog log)
        {
            return new BalanceRepository(new AzureTableStorage<TraderBalanceEntity>(connstring, "AccountBalances", log));
        }


        public static IdentityGenerator CreateIdentityGenerator(string connstring, ILog log)
        {
            return new IdentityGenerator(new AzureTableStorage<IdentityEntity>(connstring, "Setup", log));
        }



        public static OrdersRepository CreateOrdersRepository(string connstring, ILog log)
        {
            return new OrdersRepository(new AzureTableStorage<OrderEntity>(connstring, "Orders", log));
        }


        public static TraderSettingsRepository CreateTraderSettingsRepository(string connString, ILog log)
        {
            return new TraderSettingsRepository(new AzureTableStorage<TraderSettingsEntity>(connString, "TraderSettings", log));
        }


        private const string TableNameDictionaries = "Dictionaries";
        public static class Dictionaries
        {
            

            public static AssetsRepository CreateAssetsRepository(string connstring, ILog log)
            {
                return new AssetsRepository(new AzureTableStorage<AssetEntity>(connstring, TableNameDictionaries, log));
            }


            public static AssetPairsRepository CreateAssetPairsRepository(string connString, ILog log)
            {
                return new AssetPairsRepository(new AzureTableStorage<AssetPairEntity>(connString, TableNameDictionaries, log));
            }
        }

    }
}
