using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Core.Feed;
using Core.Orders;
using LykkeMarketPlace.Services;
using LykkeMarketPlace.Strings;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;

namespace LykkeMarketPlace.Hubs
{
    public class LkHub : Hub
    {
        internal static readonly DictionaryThreadSafe<string, LkHubConnection> Connections = new DictionaryThreadSafe<string, LkHubConnection>();

        public static void BroadcastPrice(AssetPairBestRate price)
        {
            var asset = Dependencies.AssetPairsDictionary.Get(price.Id);
            if (asset == null)
                return;

            var hub = GlobalHost.ConnectionManager.GetLkHub();

            var bid = price.Bid;
            var ask = price.Ask;

            hub.Clients.All.Price(new {id = price.Id, bid, ask});
        }


        public static async Task RefreshBalance(string traderId)
        {
            var connection = Connections.Where(itm => itm.TraderId == traderId).FirstOrDefault();

            if (connection == null)
                return;

            var hub = GlobalHost.ConnectionManager.GetLkHub();


            var balances = await Dependencies.BalanceAccess.GetCurrencyBalances(traderId);

            hub.Clients.Client(connection.Id)
                .RefreshBalance(balances.Select(itm => new {cur = itm.Id, val = itm.Value.MoneyToStr()}));

        }

        public static async Task RefreshOrderBooks(string asset)
        {
            var orderBooks = await GetOrderBook(asset);
            var hub = GlobalHost.ConnectionManager.GetLkHub();
            hub.Clients.All.OrderBooks(orderBooks);
        }


        private string GetTraderId()
        {
            return Context.User.Identity.Name;
        }


        public override Task OnConnected()
        {
            Connections.Add(Context.ConnectionId, LkHubConnection.Create(Context.ConnectionId, Context.User.Identity.Name));
            SendAssets();
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            var connection = LkHubConnection.Create(Context.ConnectionId, Context.User.Identity.Name);
            Connections.AddIfNotExists(Context.ConnectionId, connection);
            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Connections.RemoveIfExists(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        private void SendAssets()
        {
            Clients.Caller.Assets(
                Dependencies.AssetPairsDictionary.GetAll().Select(
                    itm => new {id = itm.Id, b = itm.BaseAssetId, q = itm.QuotingAssetId, acc = itm.Accuracy}));
        }



        private void SendMessage(string divId, string message)
        {
            Clients.Caller.Message(new {divId, message});
        }


        private static object ConvertToContractModel(LimitOrderBookModel o)
        {
            return new
            {
                asset = o.Asset,
                data =
                    o.Items.Select(
                        itm =>
                            itm.Type == BookOrderType.Bid
                                ? (object) new {b = itm.Volume, r = itm.Rate}
                                : new {a = itm.Volume, r = itm.Rate})
            };
        }


        private static async Task<object> GetOrderBook(string asset)
        {
            var orderBook = await Dependencies.SrvLimitOrderBookGenerator.GetOrderBookAsync(asset);
            return new[] { ConvertToContractModel(orderBook) };
        }


        private static async Task<object> GetOrderBooks()
        {
            var orderBooks = await Dependencies.SrvLimitOrderBookGenerator.GetOrderBooksAsync();
            return orderBooks.Select(ConvertToContractModel).ToArray();
        }

        public async Task RefreshOrderBooks()
        {
            var orders = await GetOrderBooks();
            Clients.Caller.OrderBooks(orders);
        }

        public async Task DoMarketOrder(LkeHubDealModel model)
        {
            if (model.Action != "sell" && model.Action != "buy")
            {
                SendMessage("#volume"+model.CurTo, Phrases.InvalidCommand);
                return;
            }

            double volume;

            try
            {
                volume = model.Volume.ParseAnyDouble();
            }
            catch (Exception)
            {
                SendMessage("#volume" + model.CurTo, Phrases.InvalidAmountFormat);
                return;
            }

            var orderAction = model.Action == "sell" ? OrderAction.Sell : OrderAction.Buy;
            var traderId = GetTraderId();

            var asset = Dependencies.AssetPairsDictionary.FindByBasedOrQuotingAsset(model.CurFrom, model.CurTo);

            var marketOrder = MarketOrder.Create(traderId, asset.Id, model.CurFrom, orderAction, volume);

            try
            {
                   await Dependencies.SrvOrdersRegistrator.RegisterTradeOrderAsync(marketOrder);

                SendMessage("#volume" + model.CurTo, Phrases.OrderHasBeenRegistered);
            }
            catch (Exception ex)
            {

                SendMessage("#volume" + model.CurTo, ex.Message);
            }



        }

        public async Task DoLimitOrder(LkeHubLimitOrderModel model)
        {
            var orderAction = model.Action == "sell" ? OrderAction.Sell : OrderAction.Buy;

            double volume;

            try
            {
                volume = model.Volume.ParseAnyDouble();
            }
            catch (Exception)
            {
                SendMessage("#volume" + model.CurTo, Phrases.InvalidAmountFormat);
                return;
            }

            double price;
            var id = orderAction == OrderAction.Sell ? "#slr" : "#blr";
            try
            {
                price = model.Price.ParseAnyDouble();
            }
            catch (Exception)
            {

                SendMessage(id + model.CurTo, Phrases.InvalidRateFormat);
                return;
            }


            var traderId = GetTraderId();
            var asset = Dependencies.AssetPairsDictionary.FindByBasedOrQuotingAsset(model.CurFrom, model.CurTo);
            var limitOrder = LimitOrder.Create(traderId, asset.Id, model.CurFrom, orderAction, volume, price);

            try
            {
                await Dependencies.SrvOrdersRegistrator.RegisterTradeOrderAsync(limitOrder);
                SendMessage(id + model.CurTo, Phrases.OrderHasBeenRegistered);
            }
            catch (Exception ex)
            {

                SendMessage(id + model.CurTo, ex.Message);
            }

        }

        public async Task DoLimitOrder2(LkeHubLimitOrder2Model model)
        {
            double volume;

            try
            {
                volume = model.Volume.ParseAnyDouble();
            }
            catch (Exception)
            {
                SendMessage("#volume" + model.CurTo, Phrases.InvalidAmountFormat);
                return;
            }


            double bid;

            try
            {
                bid = model.Bid.ParseAnyDouble();
            }
            catch (Exception)
            {
                SendMessage("#lobid" + model.CurTo, Phrases.InvalidRateFormat);
                return;
            }

            double ask;

            try
            {
                ask = model.Ask.ParseAnyDouble();
            }
            catch (Exception)
            {
                SendMessage("#loask" + model.CurTo, Phrases.InvalidRateFormat);
                return;
            }

            var traderId = GetTraderId();
            var asset = Dependencies.AssetPairsDictionary.FindByBasedOrQuotingAsset(model.CurFrom, model.CurTo);
            var limitOrder1 = LimitOrder.Create(traderId, asset.Id, model.CurFrom, OrderAction.Sell, volume, ask);
            var limitOrder2 = LimitOrder.Create(traderId, asset.Id, model.CurFrom, OrderAction.Buy, volume, bid);

            try
            {
                await Dependencies.SrvOrdersRegistrator.RegisterLinkedLimitOrders(limitOrder1, limitOrder2);
                SendMessage("#lobid" + model.CurTo, Phrases.OrderHasBeenRegistered);
            }
            catch (Exception ex)
            {

                SendMessage("#lobid" + model.CurTo, ex.Message);
            }


        }


    }


    public class LkHubConnection
    {
        public string Id { get; set; }
        public string TraderId { get; set; }

        public static LkHubConnection Create(string id, string traderId)
        {
            return new LkHubConnection
            {
                Id = id,
                TraderId = traderId
            };
        }

    }


    public static class ConnectionManagerExt
    {
        public static IHubContext GetLkHub(this IConnectionManager connectionManager)
        {
            return connectionManager.GetHubContext<LkHub>();
        }

    }
}