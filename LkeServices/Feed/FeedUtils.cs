using System;
using System.Collections.Generic;
using System.Linq;

using Core.Feed;
using Core.Orders;

namespace LkeServices.Feed
{
    public static class FeedUtils
    {

        public static AssetPairBestRate GetBestAssetPairPrice(IEnumerable<LimitOrder> limitOrders)
        {

            var bids = new List<LimitOrder>();
            var asks = new List<LimitOrder>();


            foreach (var limitOrder in limitOrders)
            {
                if (limitOrder.Action == OrderAction.Buy)
                    asks.Add(limitOrder);

                if (limitOrder.Action == OrderAction.Sell)
                    bids.Add(limitOrder);
            }

            if (bids.Count == 0 || asks.Count == 0)
                return null;

            var asset = bids.Count > 0 ? bids[0].Asset : asks[0].Asset;

            return new AssetPairBestRate
            {
                Id = asset,
                Ask = asks.Max(itm => itm.Price),
                Bid = bids.Min(itm => itm.Price),
                DateTime = DateTime.UtcNow
            };


        }
    }
}
