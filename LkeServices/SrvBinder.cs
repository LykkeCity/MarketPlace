﻿using Common;
using Common.IocContainer;
using Core.Assets;
using Core.Finance;
using Core.Orders;
using LkeServices.Clients;
using LkeServices.Feed;
using LkeServices.Kyc;
using LkeServices.Orders;

namespace LkeServices
{
    public static class SrvBinder
    {


        public static void BindTraderPortalServices(IoC ioc)
        {
            ioc.RegisterSingleTone<SrvClientManager>();

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


        public static void BindLykkeWalletServices(this IoC ioc)
        {
            ioc.RegisterSingleTone<SrvClientManager>();
            ioc.RegisterSingleTone<SrvKycDocumentsManager>();
            ioc.RegisterSingleTone<SrvKycStatusManager>();
        }

        public static void BindBackOfficeServices(this IoC ioc)
        {
            ioc.RegisterSingleTone<SrvClientFinder>();
            ioc.RegisterSingleTone<SrvKycDocumentsManager>();
            ioc.RegisterSingleTone<SrvKycStatusManager>();
        }


        public static void StartTraderPortalServices(this IoC ioc)
        {
            foreach (var starter in ioc.GetObjects<IStarter>())
                starter.Start();
        }
    }
}
