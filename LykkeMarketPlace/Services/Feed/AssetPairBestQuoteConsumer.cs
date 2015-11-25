using System;
using System.Threading.Tasks;
using Core.Feed;

namespace LykkeMarketPlace.Services.Feed
{
    public class AssetPairBestQuoteConsumer : IAssetPairBestPriceNotifier
    {
        private readonly Action<AssetPairBestRate> _notificationAction;

        public AssetPairBestQuoteConsumer(Action<AssetPairBestRate> notificationFunc)
        {
            _notificationAction = notificationFunc;
        }

        public Task NotifyNewAsset(AssetPairBestRate assetQuote)
        {
            _notificationAction(assetQuote);
            return Task.FromResult(0);
        }
    }
}