using System.Linq;
using System.Threading.Tasks;
using Core.Feed;
using Core.Orders;
using LkeServices.Feed;

namespace LkeServices.Orders
{
    public class SrvAssetPairBestPriceBroadcaster
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IAssetPairBestPriceNotifier[] _assetPairBestPriceNotifiers;

        public SrvAssetPairBestPriceBroadcaster(IOrdersRepository ordersRepository, IAssetPairBestPriceNotifier[] assetPairBestPriceNotifiers)
        {
            _ordersRepository = ordersRepository;
            _assetPairBestPriceNotifiers = assetPairBestPriceNotifiers;
        }


        private async Task NotiftBestPrice(AssetPairBestRate assetPairBestRate)
        {
            foreach (var assetPairBestPriceNotifier in _assetPairBestPriceNotifiers)
                await assetPairBestPriceNotifier.NotifyNewAsset(assetPairBestRate);
        }

        public async Task BroadCastBestPrices()
        {
            var orders = await _ordersRepository.GetLimitOrdersByStatusAsync(OrderStatus.Registered);

            var ordersByAsset = orders.GroupBy(itm => itm.Asset);

            foreach (var orderByAsset in ordersByAsset)
            {
                var bestPrice = FeedUtils.GetBestAssetPairPrice(orderByAsset);

                if (bestPrice != null)
                    await NotiftBestPrice(bestPrice);
            }
        }

    }
}
