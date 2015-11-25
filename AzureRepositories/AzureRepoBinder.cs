using AzureRepositories.Assets;
using AzureRepositories.Finance;
using AzureRepositories.Orders;
using AzureRepositories.Traders;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates;
using Common.IocContainer;
using Common.Log;
using Core;
using Core.Assets;
using Core.Finance;
using Core.Orders;
using Core.Traders;

namespace AzureRepositories
{
    public static class AzureRepoBinder
    {

        public static void BindAzureRepositories(IoC ioc, string connString, ILog log)
        {
            ioc.Register<ITradersRepository>(
                AzureRepoFactories.CreateTradersRepository(connString, log));

            ioc.Register<IBalanceRepository>(
                AzureRepoFactories.CreateBalanceRepository(connString, log));

            ioc.Register<IIdentityGenerator>(
                AzureRepoFactories.CreateIdentityGenerator(connString, log));

            ioc.Register<IOrdersRepository>(
                AzureRepoFactories.CreateOrdersRepository(connString, log));


            ioc.Register<ITraderSettingsRepository>(
                AzureRepoFactories.CreateTraderSettingsRepository(connString, log));

            ioc.Register<IAssetsRepository>(
                AzureRepoFactories.Dictionaries.CreateAssetsRepository(connString, log));

            ioc.Register<IAssetPairsRepository>(
                AzureRepoFactories.Dictionaries.CreateAssetPairsRepository(connString, log));

        }



        public static void BindAzureReposInMem(IoC ioc)
        {
            ioc.Register<ITradersRepository>(
                new TradersRepository(new NoSqlTableInMemory<TraderEntity>(), new NoSqlTableInMemory<AzureIndex>()));

            ioc.Register<IBalanceRepository>(
                new BalanceRepository(new NoSqlTableInMemory<TraderBalanceEntity>()));

            ioc.Register<IIdentityGenerator>(
                new IdentityGenerator(new NoSqlTableInMemory<IdentityEntity>()));

            ioc.Register<IOrdersRepository>(
                new OrdersRepository(new NoSqlTableInMemory<OrderEntity>()));

            ioc.Register<ITraderSettingsRepository>(
                new TraderSettingsRepository(new NoSqlTableInMemory<TraderSettingsEntity>()));


            var assetsRepositry = new AssetsRepository(new NoSqlTableInMemory<AssetEntity>());
            assetsRepositry.PopulateAssets();
            ioc.Register<IAssetsRepository>(assetsRepositry);

            var assetPairsRepository = new AssetPairsRepository(new NoSqlTableInMemory<AssetPairEntity>());
            assetPairsRepository.PopulateAssetPairsRepository();
            ioc.Register<IAssetPairsRepository>(assetPairsRepository);
        }


        private static void PopulateAssets(this IAssetsRepository assetsRepository)
        {
            assetsRepository.RegisterAssetAsync(Asset.Create("CHF", "Swiss Franc"));
            assetsRepository.RegisterAssetAsync(Asset.Create("USD", "US Dollar"));
            assetsRepository.RegisterAssetAsync(Asset.Create("EUR", "Euro"));
            assetsRepository.RegisterAssetAsync(Asset.Create("GBP", "British Pound"));
            assetsRepository.RegisterAssetAsync(Asset.Create("JPY", "Japanese Yen"));
            assetsRepository.RegisterAssetAsync(Asset.Create("CAD", "Canadian Dollar"));
            assetsRepository.RegisterAssetAsync(Asset.Create("AUD", "Australian Dollar"));
            assetsRepository.RegisterAssetAsync(Asset.Create("NZD", "New-Zealand Dollar"));
        }


        private static void PopulateAssetPairsRepository(this IAssetPairsRepository assetsRepository)
        {
            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "AUDNZD",
                BaseAssetId = "AUD",
                QuotingAssetId = "NZD",
                Accuracy = 5
            });




            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "AUDCAD",
                BaseAssetId = "AUD",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "AUDCHF",
                BaseAssetId = "AUD",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "AUDJPY",
                BaseAssetId = "AUD",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });


            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "AUDUSD",
                BaseAssetId = "AUD",
                QuotingAssetId = "USD",
                Accuracy = 5
            });


            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "CADCHF",
                BaseAssetId = "CAD",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "CADJPY",
                BaseAssetId = "CAD",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });


            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "CHFJPY",
                BaseAssetId = "CHF",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });


            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "EURAUD",
                BaseAssetId = "EUR",
                QuotingAssetId = "AUD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "EURCAD",
                BaseAssetId = "EUR",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "EURCHF",
                BaseAssetId = "EUR",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "EURGBP",
                BaseAssetId = "EUR",
                QuotingAssetId = "GBP",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "EURJPY",
                BaseAssetId = "EUR",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "EURNZD",
                BaseAssetId = "EUR",
                QuotingAssetId = "NZD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "EURUSD",
                BaseAssetId = "EUR",
                QuotingAssetId = "USD",
                Accuracy = 5
            });


            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "GBPAUD",
                BaseAssetId = "GBP",
                QuotingAssetId = "AUD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "GBPCAD",
                BaseAssetId = "GBP",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "GBPCHF",
                BaseAssetId = "GBP",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "GBPJPY",
                BaseAssetId = "GBP",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "GBPNZD",
                BaseAssetId = "GBP",
                QuotingAssetId = "NZD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "GBPUSD",
                BaseAssetId = "GBP",
                QuotingAssetId = "USD",
                Accuracy = 5
            });


            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "NZDCAD",
                BaseAssetId = "NZD",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "NZDCHF",
                BaseAssetId = "NZD",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "NZDJPY",
                BaseAssetId = "NZD",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "NZDUSD",
                BaseAssetId = "NZD",
                QuotingAssetId = "USD",
                Accuracy = 5
            });


            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "USDCAD",
                BaseAssetId = "USD",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "USDCHF",
                BaseAssetId = "USD",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.SaveAsync(new AssetPair
            {
                Id = "USDJPY",
                BaseAssetId = "USD",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });

        }



    }

}
