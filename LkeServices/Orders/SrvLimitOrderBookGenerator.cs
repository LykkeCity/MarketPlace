using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Orders;

namespace LkeServices.Orders
{

    public class SrvLimitOrderBookGenerator
    {
        private readonly IOrdersRepository _ordersRepository;

        public SrvLimitOrderBookGenerator(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        private static LimitOrderBookModel ConvertOrderBook(string asset, LimitOrder[] orders)
        {
            var bid = orders.Where(o => o.Action == OrderAction.Buy)
                        .GroupBy(o => o.Price)
                        .Select(
                            o =>
                                new LimitOrderBookItem
                                {
                                    Rate = o.Key,
                                    Volume = o.Sum(i => i.Volume),
                                    Type = BookOrderType.Bid
                                })
                        .ToArray();


            var ask = orders.Where(o => o.Action == OrderAction.Sell)
                .GroupBy(o => o.Price)
                .Select(
                    o =>
                        new LimitOrderBookItem
                        {
                            Rate = o.Key,
                            Volume = o.Sum(i => i.Volume),
                            Type = BookOrderType.Ask
                        })
                .ToArray();

            var items = new List<LimitOrderBookItem>(bid.Length + ask.Length + 1);

            items.AddRange(bid);
            items.AddRange(ask);

            return new LimitOrderBookModel
            {
                Asset = asset,
                Items = items.OrderByDescending(itm => itm.Rate).ToArray()
            };

        }


        public async Task<LimitOrderBookModel> GetOrderBookAsync(string asset)
        {
            var orders = (await _ordersRepository.GetOrderByStatusAsync(OrderStatus.Registered, asset))
                    .Where(itm => itm is LimitOrder)
                    .Cast<LimitOrder>()
                    .ToArray();

            return ConvertOrderBook(asset, orders);
        }

        public async Task<LimitOrderBookModel[]> GetOrderBooksAsync()
        {
            var orders =
                (await _ordersRepository.GetOrdersByStatusAsync(OrderStatus.Registered))
                    .Where(itm => itm is LimitOrder)
                    .Cast<LimitOrder>()
                    .ToArray();

            return orders.GroupBy(itm => itm.Asset)
                .Select(ords =>ConvertOrderBook(ords.Key, orders.ToArray())).ToArray();
        }
    }
}
