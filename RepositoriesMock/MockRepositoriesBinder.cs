using AzureStorage.Tables;
using Common.IocContainer;
using Common.Log;
using Core.BitCoin;
using RepositoriesMock.BitCoin;

namespace RepositoriesMock
{
    public static class MockAzureRepositoriesBinder
    {
        public static void BindMockAzureRepositories(this IoC ioc, string connString, ILog log)
        {
            ioc.Register<IMockLykkeWalletRepository>(
                new MockLykkeWalletsRepository(new AzureTableStorage<MockLykkeAccountEntity>(connString, "MockLykkeWallets", log)
                    ));
        }

        public static void BinMockAzureDebug(this IoC ioc)
        {
            var localHost = @"http://127.0.0.1:8998";

            ioc.Register<IMockLykkeWalletRepository>(
                new MockLykkeWalletsRepository(new AzureTableStorageLocal<MockLykkeAccountEntity>(localHost, "MockLykkeWallets")));
        }
    }
}
