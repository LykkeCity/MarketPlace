using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Common;
using Core.Orders;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories.Orders
{


    public class OrderEntity : TableEntity
    {

        private static readonly Dictionary<string, Type> OrderTypes = new Dictionary<string, Type>
        {
            {"Market", typeof(MarketOrder) },
            {"Limit", typeof(LimitOrder) },

        };



        public static string GeneratePartitionKey(string traderId)
        {
            return traderId;
        }

        public static string GeneratePartitionAsAllOrders(OrderStatus status)
        {
            return status.ToString();
        }

        public static string GenerateRowKey(int id)
        {
            return id.ToString("0000000000");
        }

        public string OrderData { get; set; }
        public string Type { get; set; }


        public void SetOrder(OrderBase order)
        {
            foreach (var orderType in OrderTypes)
                if (orderType.Value == order.GetType())
                {
                    Type = orderType.Key;
                    OrderData = Newtonsoft.Json.JsonConvert.SerializeObject(order);
                    return;
                }

            throw new Exception("Can not set order. Unknown order type: "+order.GetType());
        }


        public OrderBase GetOrder()
        {
            if (!OrderTypes.ContainsKey(Type))
                throw new Exception("Can not get order. Unknown order type: " + Type);

            var type = OrderTypes[Type];
            return (OrderBase) Newtonsoft.Json.JsonConvert.DeserializeObject(OrderData, type);
        }


        public static OrderEntity Create(OrderBase src)
        {
            var result = new OrderEntity
            {
                PartitionKey = GeneratePartitionAsAllOrders(src.Status),
                RowKey = GenerateRowKey(src.Id)
            };

            result.SetOrder(src);

            return result;
        }

    }

    public class OrdersRepository : IOrdersRepository
    {
        private readonly INoSQLTableStorage<OrderEntity> _tableStorage;

        public OrdersRepository(INoSQLTableStorage<OrderEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task RegisterOrderAsync(OrderBase order)
        {
            var newEntity = OrderEntity.Create(order);
            await _tableStorage.InsertAsync(newEntity);

            newEntity.PartitionKey = OrderEntity.GeneratePartitionKey(order.TraderId);
            await _tableStorage.InsertAsync(newEntity);
        }

        public async Task<IEnumerable<OrderBase>> GetOrdersByStatusAsync(OrderStatus orderStatus)
        {
            var partitionKey = OrderEntity.GeneratePartitionAsAllOrders(orderStatus);
            var entities = await _tableStorage.GetDataAsync(partitionKey);

            return entities.Select(itm => itm.GetOrder());
        }

        public async Task<IEnumerable<LimitOrder>> GetLimitOrdersByStatusAsync(OrderStatus orderStatus)
        {
            var partitionKey = OrderEntity.GeneratePartitionAsAllOrders(orderStatus);

            var result = new List<LimitOrder>();

            await _tableStorage.GetDataByChunksAsync(partitionKey, 
                chunk => 
                result.AddRange(chunk.Select(orderEntity => orderEntity.GetOrder()).OfType<LimitOrder>()));
            return result;
        }

        public async Task<IEnumerable<OrderBase>> GetOrderByStatusAsync(OrderStatus orderStatus, string asset)
        {
            var partitionKey = OrderEntity.GeneratePartitionAsAllOrders(orderStatus);

            var result = await _tableStorage.ScanAndGetList(partitionKey, itm =>
            {
                var order = itm.GetOrder() as LimitOrder;
                if (order == null)
                    return false;

                return order.Asset == asset;
            });

            return result.Select(itm => itm.GetOrder());
        }

        public async Task<IEnumerable<OrderBase>> GetOrdersByTraderAsync(string traderId, Func<OrderBase, bool> filter)
        {
            var partitionKey = OrderEntity.GeneratePartitionKey(traderId);

            var result = filter == null 
                ? await _tableStorage.GetDataAsync(partitionKey)
                :await _tableStorage.ScanAndGetList(partitionKey, itm => filter(itm.GetOrder()));

            return result.Select(itm => itm.GetOrder());
        }

        public async Task UpdateOrderAsync(OrderBase updatedOrder)
        {
            var partitionKey = OrderEntity.GeneratePartitionKey(updatedOrder.TraderId);
            var rowKey = OrderEntity.GenerateRowKey(updatedOrder.Id);


            var oldStatus = OrderStatus.Registered;

            await _tableStorage.ReplaceAsync(partitionKey, rowKey, itm =>
            {
                oldStatus = itm.GetOrder().Status;
                itm.SetOrder(updatedOrder);
                return itm;
            });



            if (oldStatus == updatedOrder.Status)
            {
                await _tableStorage.ReplaceAsync(OrderEntity.GeneratePartitionAsAllOrders(oldStatus), rowKey, itm =>
                {
                    itm.SetOrder(updatedOrder);
                    return itm;
                });
                return;
            }


            partitionKey = OrderEntity.GeneratePartitionAsAllOrders(oldStatus);
            await _tableStorage.DeleteAsync(partitionKey, rowKey);

            var newEntity = OrderEntity.Create(updatedOrder);
            await _tableStorage.InsertOrReplaceAsync(newEntity);



        }
    }
}
