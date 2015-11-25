using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Core.Orders
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderAction
    {
        Sell, Buy
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderStatus
    {
        Registered, Done, Canceled
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum OrderFailReason
    {
        None, NotEnoughFunds
    }

    public class OrderBase
    {
        public string TraderId { get; set; }
        public int Id { get; set; }
        public DateTime Registered { get; set; }
        public DateTime? Executed { get; set; }
        public OrderStatus Status { get; set; }
        public OrderFailReason FailReason { get; set; }
    }


    public class TraderOrderBase : OrderBase
    {
        public string Asset { get; set; }
        public double Volume { get; set; }
        public OrderAction Action { get; set; }
        public string BaseCurrency { get; set; }

        public int[] MatchedOrders { get; set; }
        public double Exchanged { get; set; }

        public double FullVolume => Volume - Exchanged;


        public void AddExchangingOrder(int orderId)
        {
            var orders = MatchedOrders?.ToList() ?? new List<int>();
            orders.Add(orderId);
            MatchedOrders = orders.ToArray();
        }


        public bool IsOrderFullfiled()
        {
            return Math.Abs(FullVolume) < 0.001;
        }

    }

    public class MarketOrder : TraderOrderBase
    {



        public static MarketOrder Create(string traderId, string asset, string baseCurrency, OrderAction action, double volume)
        {
            return new MarketOrder
            {
                TraderId = traderId,
                Asset = asset,
                Action = action,
                Volume = volume,
                BaseCurrency = baseCurrency
            };
        }

    }

    public class LimitOrder : TraderOrderBase
    {
        /// <summary>
        /// -1 - if order is not BothSided; 
        /// </summary>
        public int LinkedLimitOrder { get; set; }
        public double Price { get; set; }

        public static LimitOrder Create(string traderId, string asset, string baseCurrency, OrderAction action, double volume, double price)
        {
            return new LimitOrder
            {
                TraderId = traderId,
                Asset = asset,
                Action = action,
                Volume = volume,
                Price = price,
                BaseCurrency = baseCurrency
            };
        }
    }



    public interface IOrdersRepository
    {

        Task RegisterOrderAsync(OrderBase order);
        Task<IEnumerable<OrderBase>> GetOrdersByStatusAsync(OrderStatus orderStatus);
        Task<IEnumerable<LimitOrder>> GetLimitOrdersByStatusAsync(OrderStatus orderStatus);
        Task<IEnumerable<OrderBase>> GetOrderByStatusAsync(OrderStatus orderStatus, string asset);
        Task<IEnumerable<OrderBase>> GetOrdersByTraderAsync(string traderId, Func<OrderBase, bool> filter);
        Task UpdateOrderAsync(OrderBase order);
    }

}
