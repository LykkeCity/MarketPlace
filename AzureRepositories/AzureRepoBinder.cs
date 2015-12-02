using Common.IocContainer;
using Common.Log;
using Core;
using Core.Assets;
using Core.BackOffice;
using Core.Clients;
using Core.Finance;
using Core.Kyc;
using Core.Orders;

namespace AzureRepositories
{
    public static class AzureRepoBinder
    {

        public static void BindAzureRepositories(this IoC ioc, string connString, string connStringBackOffice, ILog log)
        {
            ioc.Register<IClientAccountsRepository>(
                AzureRepoFactories.Clients.CreateTradersRepository(connString, log));

            ioc.Register<IPersonalDataRepository>(
                AzureRepoFactories.Clients.CreatePersonalDataRepository(connString, log));

            ioc.Register<IKycRepository>(
                AzureRepoFactories.Clients.CreateKycRepository(connString, log));

            ioc.Register<IKycDocumentsRepository>(
                AzureRepoFactories.Clients.CreateKycDocumentsRepository(connString, log));

            ioc.Register<IKycDocumentsScansRepository>(
                AzureRepoFactories.Clients.CreatKycDocumentsScansRepository(connString));

            ioc.Register<IKycUploadsLog>(
                AzureRepoFactories.Clients.CreateKycUploadsLog(connString, log));

            ioc.Register<IBalanceRepository>(
                AzureRepoFactories.CreateBalanceRepository(connString, log));

            ioc.Register<IIdentityGenerator>(
                AzureRepoFactories.CreateIdentityGenerator(connString, log));

            ioc.Register<IOrdersRepository>(
                AzureRepoFactories.CreateOrdersRepository(connString, log));


            ioc.Register<IClientSettingsRepository>(
                AzureRepoFactories.CreateTraderSettingsRepository(connString, log));

            ioc.Register<IAssetsRepository>(
                AzureRepoFactories.Dictionaries.CreateAssetsRepository(connString, log));

            ioc.Register<IAssetPairsRepository>(
                AzureRepoFactories.Dictionaries.CreateAssetPairsRepository(connString, log));

            ioc.Register<IBrowserSessionsRepository>(
                AzureRepoFactories.CreateBrowserSessionsRepository(connString, log));

            ioc.Register<IMenuBadgesRepository>(
                AzureRepoFactories.BackOffice.CreateMenuBadgesRepository(connStringBackOffice, log));

        }

    }

}
