using Common;
using Common.IocContainer;
using Core.Assets;
using Core.Finance;
using Core.Orders;
using LkeServices.Feed;
using LkeServices.Orders;

namespace LkeServices
{
    public static class SrvBinder
    {

        public static void BindTraderPortalServices(IoC ioc)
        {
            ioc.RegisterSingleTone<SrvBalanceAccess>();
            ioc.RegisterSingleTone<SrvOrdersRegistrator>();
            ioc.RegisterSingleTone<SrvOrdersExecutor>();
            ioc.SelfBond<IOrderExecuter, SrvOrdersExecutor>();
            ioc.RegisterSingleTone<SrvLimitOrderBookGenerator>();
            ioc.RegisterSingleTone<AssetPairsBestRateCache>();

            ioc.RegisterSingleTone<SrvAssetPairBestPriceBroadcaster>();

            ioc.RegisterSingleTone<AssetDictionary>();
            ioc.SelfBond<IAssetsDictionary, AssetDictionary>();
            ioc.SelfBond<IStarter, AssetDictionary>();

            ioc.RegisterSingleTone<AssetPairsDictionary>();
            ioc.SelfBond<IAssetPairsDictionary, AssetPairsDictionary>();
            ioc.SelfBond<IStarter, AssetPairsDictionary>();
        }


        public static void StartTraderPortalServices(IoC ioc)
        {
            foreach (var starter in ioc.GetObjects<IStarter>())
                starter.Start();
        }
    }
}
