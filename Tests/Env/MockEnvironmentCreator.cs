using System;
using AzureRepositories;
using Common.IocContainer;
using Common.Log;
using Core.Clients;
using Core.Feed;
using Core.Finance;
using Core.Orders;
using LkeServices;
using LkeServices.Feed;

namespace Tests.Env
{
    public static class MockEnvironmentCreator
    {

        public static IoC Create()
        {

            var ioc = new IoC();
            var log = new LogToConsole();

            ioc.Register<ILog>(log);

            AzureRepoBinder.BindAzureReposInMem(ioc);
            SrvBinder.BindTraderPortalServices(ioc);

            ioc.RegisterSingleTone<OrderBookChangesRegistrator>();
            ioc.SelfBond<IOrderBookChangesConsumer, OrderBookChangesRegistrator>();

            ioc.RegisterSingleTone<AssetPairBestPricesChangesRegistrator>();
            ioc.SelfBond<IAssetPairBestPriceNotifier, AssetPairBestPricesChangesRegistrator>();

            return ioc;

        }


        public static IClientAccount RegisterTrader(this IoC ioc, string email)
        {
            var trader = new ClientAccount {Email = email};

            return ioc.GetObject<IClientAccountsRepository>().RegisterAsync(trader, "123").Result;
        }


        public static void SetMarketPrice(this IoC ioc, string asset, double bid, double ask)
        {
            ioc.GetObject<AssetPairsBestRateCache>().NotifyNewAsset(new AssetPairBestRate { DateTime = DateTime.UtcNow, Ask = ask, Bid = bid, Id = asset});
        }


        public static void DepositAcount(this IoC ioc, string traderId, string currency, double amount)
        {
            ioc.GetObject<SrvBalanceAccess>().ChangeBalance(traderId, currency, amount);
        }


        public static double GetBalance(this IoC ioc, string traderId, string currency)
        {
            return ioc.GetObject<SrvBalanceAccess>().GetCurrencyBalances(traderId).Result.GetBalance(currency);
        }

    }
}
