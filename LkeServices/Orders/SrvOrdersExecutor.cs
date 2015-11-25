using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Assets;
using Core.Finance;
using Core.Orders;

namespace LkeServices.Orders
{
    public class SrvOrdersExecutor : IOrderExecuter
    {
        public class OrderWrapper
        {
            public LimitOrder Order { get; private set; }
            public double Rank;

            public static OrderWrapper Create(LimitOrder order)
            {
                return new OrderWrapper
                {
                    Order = order
                };
            }
        }

        public class OrderList
        {
            private readonly IAssetPairsDictionary _assetPairsDictionary;

            public List<MarketOrder> MarketOrders = new List<MarketOrder>();
            public List<OrderWrapper> LimitOrdersBuy = new List<OrderWrapper>();
            public List<OrderWrapper> LimitOrdersSell = new List<OrderWrapper>();

            public OrderList(IAssetPairsDictionary assetPairsDictionary)
            {
                _assetPairsDictionary = assetPairsDictionary;
            }


            public int CalcWorstSpread(string asset)
            {
                var buyOrders = LimitOrdersBuy.Where(itm => itm.Order.Asset == asset).ToArray();
                var sellOrders = LimitOrdersSell.Where(itm => itm.Order.Asset == asset).ToArray();

                if (buyOrders.Length == 0 || sellOrders.Length == 0)
                    return 0;


                return (int)(buyOrders.Max(itm => itm.Order.Price) - sellOrders.Min(itm => itm.Order.Price))* _assetPairsDictionary.Get(asset).Multiplier();

            }


            private static void CleanUpOrderWrapper(List<OrderWrapper> orderWrappers)
            {
                var itemsToDelete = orderWrappers.Where(itm => itm.Order.Status != OrderStatus.Registered).ToArray();

                foreach (var toDelete in itemsToDelete)
                    orderWrappers.Remove(toDelete);
            }

            private void CleanUpMarketOrders()
            {
                var itemsToDelete = MarketOrders.Where(itm => itm.Status != OrderStatus.Registered).ToArray();

                foreach (var toDelete in itemsToDelete)
                    MarketOrders.Remove(toDelete);
            }

            public void CleanUpFullfiledLimitOrders()
            {
                CleanUpOrderWrapper(LimitOrdersBuy);
                CleanUpOrderWrapper(LimitOrdersSell);
                CleanUpMarketOrders();

            }


        }

        private readonly IOrdersRepository _ordersRepository;
        private readonly SrvBalanceAccess _srvBalanceAccess;
        private readonly IAssetPairsDictionary _assetPairsCachedReader;
        private readonly SrvAssetPairBestPriceBroadcaster _srvAssetPairBestPriceBroadcaster;
        private readonly IOrderBookChangesConsumer[] _orderFlowFeedNotifiers;

        public SrvOrdersExecutor(IOrdersRepository ordersRepository, SrvBalanceAccess srvBalanceAccess, 
            IAssetPairsDictionary assetPairsCachedReader, SrvAssetPairBestPriceBroadcaster srvAssetPairBestPriceBroadcaster, IOrderBookChangesConsumer[] orderFlowFeedNotifiers)
        {
            _ordersRepository = ordersRepository;
            _srvBalanceAccess = srvBalanceAccess;
            _assetPairsCachedReader = assetPairsCachedReader;
            _srvAssetPairBestPriceBroadcaster = srvAssetPairBestPriceBroadcaster;
            _orderFlowFeedNotifiers = orderFlowFeedNotifiers;
        }



        private readonly List<Func<string, Task>> _balanceChangeActions = new List<Func<string, Task>>();
        public void RegisterBalanceChange(Func<string, Task> action)
        {
            _balanceChangeActions.Add(action);
        }

        private async Task UpdateBalanceChange(string traderId)
        {
            foreach (var balanceChangeAction in _balanceChangeActions)
                await balanceChangeAction(traderId);
        }

        private OrderList SeparateOrders(OrderBase[] orders)
        {

            var result = new OrderList(_assetPairsCachedReader);

            foreach (var orderBase in orders)
            {
                var marketOrder = orderBase as MarketOrder;
                if (marketOrder != null)
                    result.MarketOrders.Add(marketOrder);

                var limitOrder = orderBase as LimitOrder;
                if (limitOrder != null)
                {
                    if (limitOrder.Action == OrderAction.Buy)
                        result.LimitOrdersBuy.Add(OrderWrapper.Create(limitOrder)); 

                    if (limitOrder.Action == OrderAction.Sell)
                        result.LimitOrdersSell.Add(OrderWrapper.Create(limitOrder));
                }
            }

            result.MarketOrders = result.MarketOrders.OrderBy(itm => itm.Id).ToList();
            result.LimitOrdersBuy = result.LimitOrdersBuy.OrderByDescending(itm => itm.Order.Price).ToList();
            result.LimitOrdersSell = result.LimitOrdersSell.OrderBy(itm => itm.Order.Price).ToList();

            return result;
        }


        private async Task<bool> CheckIfEnoughBalance(IAssetPair assetPair, TraderOrderBase order, double exchangedVolume)
        {
            var balances = await _srvBalanceAccess.GetCurrencyBalances(order.TraderId);

            if (order.Action == OrderAction.Buy)
                return (balances.GetBalance(assetPair.BaseAssetId) - exchangedVolume) >= 0;

            return (balances.GetBalance(assetPair.QuotingAssetId) - order.Volume) >= 0;
        }


        private async Task ChangeBalance(IAssetPair assetPair, TraderOrderBase order, double exchangedVolume)
        {
            if (order.Action == OrderAction.Buy)
            {
                await _srvBalanceAccess.ChangeBalance(order.TraderId, assetPair.QuotingAssetId, order.Volume);
                await _srvBalanceAccess.ChangeBalance(order.TraderId, assetPair.BaseAssetId, -exchangedVolume);
            }
            else
            {
                await _srvBalanceAccess.ChangeBalance(order.TraderId, assetPair.QuotingAssetId, -order.Volume);
                await _srvBalanceAccess.ChangeBalance(order.TraderId, assetPair.BaseAssetId, exchangedVolume);
            }

            await UpdateBalanceChange(order.TraderId);
        }


        private static LimitOrder[] GetRankedOrders(MarketOrder marketOrder, OrderList orderList)
        {
            var worsSpread = orderList.CalcWorstSpread(marketOrder.Asset);

            if (worsSpread <= 0)
                return marketOrder.Action == OrderAction.Buy
                    ? orderList.LimitOrdersSell.Where(
                        itm => itm.Order.Asset == marketOrder.Asset && itm.Order.TraderId != marketOrder.TraderId)
                        .OrderBy(itm => itm.Order.Price)
                        .Select(itm => itm.Order)
                        .ToArray()
                    : orderList.LimitOrdersBuy.Where(
                        itm => itm.Order.Asset == marketOrder.Asset && itm.Order.TraderId != marketOrder.TraderId)
                        .OrderByDescending(itm => itm.Order.Price)
                        .Select(itm => itm.Order)
                        .ToArray();


            var resultOrderList = marketOrder.Action == OrderAction.Buy
                ? orderList.LimitOrdersSell.Where(
                    itm => itm.Order.Asset == marketOrder.Asset && itm.Order.TraderId != marketOrder.TraderId).ToArray()
                : orderList.LimitOrdersBuy.Where(
                    itm => itm.Order.Asset == marketOrder.Asset && itm.Order.TraderId != marketOrder.TraderId).ToArray();

            foreach (var orderWrapper in resultOrderList)
                orderWrapper.Rank = orderWrapper.Order.Price;


            return marketOrder.Action == OrderAction.Buy
                ? resultOrderList.OrderBy(itm => itm.Rank).Select(itm => itm.Order).ToArray()
                : resultOrderList.OrderByDescending(itm => itm.Rank).Select(itm => itm.Order).ToArray();

        }


        private static double GetExchangeVolume(TraderOrderBase order1, TraderOrderBase order2)
        {
            return order1.FullVolume > order2.FullVolume
                ? order2.FullVolume
                : order1.FullVolume;

        }



        private static double GetOrderPrice(TraderOrderBase order1, TraderOrderBase order2)
        {
            var limit1 = order1 as LimitOrder;
            var limit2 = order2 as LimitOrder;

            if (limit1 == null && limit2 != null)
                return limit2.Price;

            if (limit2 == null && limit1 != null)
                return limit1.Price;

            if (limit1 == null && limit2 == null)
                throw new Exception("Both orders are market. Something is defenetly wrong.");

            return limit1.Id < limit2.Id ? limit1.Price : limit2.Price;
        }


        private async Task ExecuteOrders(TraderOrderBase order1, TraderOrderBase order2)
        {
            var exchangeVolume = GetExchangeVolume(order1, order2);

            var price = GetOrderPrice(order1, order2);

            var assetPair =  _assetPairsCachedReader.Get(order1.Asset);

            var marketOrderExchangeVolume = order1.BaseCurrency == assetPair.BaseAssetId
                ? exchangeVolume * price
                : exchangeVolume / price;


            var limitOrderExchangeVolume = order2.BaseCurrency == assetPair.BaseAssetId
                ? exchangeVolume * price
                : exchangeVolume / price;


            if (!await CheckIfEnoughBalance(assetPair, order1, marketOrderExchangeVolume))
            {
                order1.Status = OrderStatus.Canceled;
                order1.FailReason = OrderFailReason.NotEnoughFunds;
                await _ordersRepository.UpdateOrderAsync(order1);
                return;
            }

            if (!await CheckIfEnoughBalance(assetPair, order2, limitOrderExchangeVolume))
            {
                order2.Status = OrderStatus.Canceled;
                order2.FailReason = OrderFailReason.NotEnoughFunds;
                await _ordersRepository.UpdateOrderAsync(order2);
                return;
            }


            order1.Exchanged += exchangeVolume;
            order2.Exchanged += exchangeVolume;

            order1.AddExchangingOrder(order2.Id);
            order2.AddExchangingOrder(order1.Id);

            if (order1.IsOrderFullfiled())
                order1.Status = OrderStatus.Done;


            await _ordersRepository.UpdateOrderAsync(order1);

            if (order2.IsOrderFullfiled())
                order2.Status = OrderStatus.Done;

            await _ordersRepository.UpdateOrderAsync(order2);

            await ChangeBalance(assetPair, order1, marketOrderExchangeVolume);
            await ChangeBalance(assetPair, order2, limitOrderExchangeVolume);

            await NotifyOrderBookChanged(assetPair.Id);
        }


        private async Task ExecuteMarketOrders(OrderList orderList)
        {
            var marketOrders = orderList.MarketOrders.ToArray();

            foreach (var marketOrder in marketOrders)
                while (!marketOrder.IsOrderFullfiled() && marketOrder.Status == OrderStatus.Registered)
                    try
                    {
                        var limitOrders = GetRankedOrders(marketOrder, orderList);

                        //If there is no limit orders to execute the market order - exit from the loop
                        if (limitOrders == null || limitOrders.Length == 0)
                            break;

                        foreach (var limitOrder in limitOrders)
                            await ExecuteOrders(marketOrder, limitOrder);

                    }
                    finally
                    {
                        orderList.CleanUpFullfiledLimitOrders();
                    }

        }


        private async Task ExecuteLimitOrders(OrderList orderList)
        {
            foreach (var orderWrapperBuy in orderList.LimitOrdersBuy.OrderBy(itm => itm.Order.Id))
                foreach (var orderWrapperSell in orderList.LimitOrdersSell.OrderBy(itm => itm.Order.Id))
                    if (orderWrapperBuy.Order.Price >= orderWrapperSell.Order.Price && 
                        orderWrapperBuy.Order.Asset == orderWrapperSell.Order.Asset &&
                        orderWrapperBuy.Order.TraderId != orderWrapperSell.Order.TraderId)
                        try
                        {
                            await ExecuteOrders(orderWrapperBuy.Order, orderWrapperSell.Order);
                        }
                        finally
                        {
                            orderList.CleanUpFullfiledLimitOrders();
                        }
        }



        private async Task NotifyOrderBookChanged(string asset)
        {
            foreach (var orderFlowNotifier in _orderFlowFeedNotifiers)
                await orderFlowNotifier.OrderBookChanged(asset);
        }

        public async Task Execute()
        {
            var orders = (await _ordersRepository.GetOrdersByStatusAsync(OrderStatus.Registered)).ToArray();

            var orderList = SeparateOrders(orders);
            await ExecuteMarketOrders(orderList);
            await ExecuteLimitOrders(orderList);
            await _srvAssetPairBestPriceBroadcaster.BroadCastBestPrices();
        }

    }

}
