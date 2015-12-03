using AzureRepositories.Accounts;
using AzureRepositories.Assets;
using AzureRepositories.BackOffice;
using AzureRepositories.Clients;
using AzureRepositories.Finance;
using AzureRepositories.Kyc;
using AzureRepositories.Orders;
using AzureStorage;
using AzureStorage.Blob;
using AzureStorage.Tables;
using AzureStorage.Tables.Templates.Index;
using Common.IocContainer;
using Core;
using Core.Accounts;
using Core.Assets;
using Core.BackOffice;
using Core.Clients;
using Core.Finance;
using Core.Kyc;
using Core.Orders;

namespace AzureRepositories
{
    public static class AzureRepoMockBinder
    {

        public static void BindAzureReposInMem(this IoC ioc)
        {
            var localHost = @"http://127.0.0.1:8998";

            ioc.Register<IClientAccountsRepository>(
                new ClientsRepository(
                    new AzureTableStorageLocal<ClientAccountEntity>(localHost, "Clients"), new AzureTableStorageLocal<AzureIndex>(localHost, "Clients")));

            ioc.Register<IPersonalDataRepository>(
                new PersonalDataRepository(new AzureTableStorageLocal<PersonalDataEntity>(localHost, "PersonalData")));

            ioc.Register<IKycRepository>(
                new KycRepository(new AzureTableStorageLocal<KycEntity>(localHost, "KycRepository")));


            ioc.Register<IKycDocumentsRepository>(
                new KycDocumentsRepository(new AzureTableStorageLocal<KycDocumentEntity>(localHost, "KycDocumentsRepository")));

            ioc.Register<IKycDocumentsScansRepository>(
                new KycDocumentsScansRepository(new AzureBlobLocal(localHost)));

            ioc.Register<IKycUploadsLog>(
               new KycUploadsLog(new AzureTableStorageLocal<KycUploadsLogItemEntity>(localHost, "KycUploadsLog")));

            ioc.Register<IBalanceRepository>(
                new BalanceRepository(new AzureTableStorageLocal<TraderBalanceEntity>(localHost, "BalanceRepository")));

            ioc.Register<IIdentityGenerator>(
                new IdentityGenerator(new AzureTableStorageLocal<IdentityEntity>(localHost, "IdentityGenerator")));

            ioc.Register<IOrdersRepository>(
                new OrdersRepository(new AzureTableStorageLocal<OrderEntity>(localHost, "OrdersRepository")));

            ioc.Register<IClientSettingsRepository>(
                new ClientSettingsRepository(new AzureTableStorageLocal<ClientSettingsEntity>(localHost, "ClientSettingsRepository")));

            var assetsRepositry = new AssetsRepository(new AzureTableStorageLocal<AssetEntity>(localHost, "AssetsRepository"));
            assetsRepositry.PopulateAssets();
            ioc.Register<IAssetsRepository>(assetsRepositry);

            var assetPairsRepository = new AssetPairsRepository(new AzureTableStorageLocal<AssetPairEntity>(localHost, "AssetPairsRepository"));
            assetPairsRepository.PopulateAssetPairsRepository();
            ioc.Register<IAssetPairsRepository>(assetPairsRepository);

            ioc.Register<IBrowserSessionsRepository>(
                new BrowserSessionsRepository(new AzureTableStorageLocal<BrowserSessionEntity>(localHost, "BrowserSessionsRepository")));

            ioc.Register<IMenuBadgesRepository>(
                new MenuBadgesRepository(new AzureTableStorageLocal<MenuBadgeEntity>(localHost, "MenuBadgesRepository")));


            ioc.Register<IAccountsRepository>(
                new AccountsRepository(new AzureTableStorageLocal<AccountEntity>(localHost, "Accounts")));
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
            assetsRepository.AddAsync(new AssetPair
            {
                Id = "AUDNZD",
                BaseAssetId = "AUD",
                QuotingAssetId = "NZD",
                Accuracy = 5
            });




            assetsRepository.AddAsync(new AssetPair
            {
                Id = "AUDCAD",
                BaseAssetId = "AUD",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "AUDCHF",
                BaseAssetId = "AUD",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "AUDJPY",
                BaseAssetId = "AUD",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });


            assetsRepository.AddAsync(new AssetPair
            {
                Id = "AUDUSD",
                BaseAssetId = "AUD",
                QuotingAssetId = "USD",
                Accuracy = 5
            });


            assetsRepository.AddAsync(new AssetPair
            {
                Id = "CADCHF",
                BaseAssetId = "CAD",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "CADJPY",
                BaseAssetId = "CAD",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });


            assetsRepository.AddAsync(new AssetPair
            {
                Id = "CHFJPY",
                BaseAssetId = "CHF",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });


            assetsRepository.AddAsync(new AssetPair
            {
                Id = "EURAUD",
                BaseAssetId = "EUR",
                QuotingAssetId = "AUD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "EURCAD",
                BaseAssetId = "EUR",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "EURCHF",
                BaseAssetId = "EUR",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "EURGBP",
                BaseAssetId = "EUR",
                QuotingAssetId = "GBP",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "EURJPY",
                BaseAssetId = "EUR",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "EURNZD",
                BaseAssetId = "EUR",
                QuotingAssetId = "NZD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "EURUSD",
                BaseAssetId = "EUR",
                QuotingAssetId = "USD",
                Accuracy = 5
            });


            assetsRepository.AddAsync(new AssetPair
            {
                Id = "GBPAUD",
                BaseAssetId = "GBP",
                QuotingAssetId = "AUD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "GBPCAD",
                BaseAssetId = "GBP",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "GBPCHF",
                BaseAssetId = "GBP",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "GBPJPY",
                BaseAssetId = "GBP",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "GBPNZD",
                BaseAssetId = "GBP",
                QuotingAssetId = "NZD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "GBPUSD",
                BaseAssetId = "GBP",
                QuotingAssetId = "USD",
                Accuracy = 5
            });


            assetsRepository.AddAsync(new AssetPair
            {
                Id = "NZDCAD",
                BaseAssetId = "NZD",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "NZDCHF",
                BaseAssetId = "NZD",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "NZDJPY",
                BaseAssetId = "NZD",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "NZDUSD",
                BaseAssetId = "NZD",
                QuotingAssetId = "USD",
                Accuracy = 5
            });


            assetsRepository.AddAsync(new AssetPair
            {
                Id = "USDCAD",
                BaseAssetId = "USD",
                QuotingAssetId = "CAD",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "USDCHF",
                BaseAssetId = "USD",
                QuotingAssetId = "CHF",
                Accuracy = 5
            });

            assetsRepository.AddAsync(new AssetPair
            {
                Id = "USDJPY",
                BaseAssetId = "USD",
                QuotingAssetId = "JPY",
                Accuracy = 3
            });

        }


    }
}
