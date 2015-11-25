using System.Linq;
using Core.Orders;
using LkeServices.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Env;

namespace Tests
{
    [TestClass]
    public class OrderRegistrationsTest
    {
        [TestMethod]
        public void TestSellLimitOrder()
        {
            var ioc = MockEnvironmentCreator.Create();

            var trader = ioc.RegisterTrader("test@test.tt");

            var srvOrderRegistrator = ioc.GetObject<SrvOrdersRegistrator>();

            ioc.DepositAcount(trader.Id, "USD", 10000);

            var limitOrder = LimitOrder.Create(trader.Id, "EURUSD", "USD", OrderAction.Sell, 1000, 1.55555);
            srvOrderRegistrator.RegisterTradeOrderAsync(limitOrder).Wait();

            var orderBook = ioc.GetObject<SrvLimitOrderBookGenerator>().GetOrderBooksAsync().Result;


            var eurUsdOrderBook = orderBook.First(itm => itm.Asset == "EURUSD");

            Assert.AreEqual(1000, eurUsdOrderBook.Items[0].Volume);
            Assert.AreEqual(1.55555, eurUsdOrderBook.Items[0].Rate);
            Assert.AreEqual(BookOrderType.Ask, eurUsdOrderBook.Items[0].Type);

        }


        [TestMethod]
        public void TestBuyLimitOrder()
        {
            var ioc = MockEnvironmentCreator.Create();

            var trader = ioc.RegisterTrader("test@test.tt");

            var srvOrderRegistrator = ioc.GetObject<SrvOrdersRegistrator>();

            ioc.DepositAcount(trader.Id, "USD", 10000);

            var limitOrder = LimitOrder.Create(trader.Id, "EURUSD", "USD", OrderAction.Buy, 1000, 1.55555);
            srvOrderRegistrator.RegisterTradeOrderAsync(limitOrder).Wait();

            var orderBook = ioc.GetObject<SrvLimitOrderBookGenerator>().GetOrderBooksAsync().Result;


            var eurUsdOrderBook = orderBook.First(itm => itm.Asset == "EURUSD");

            Assert.AreEqual(1000, eurUsdOrderBook.Items[0].Volume);
            Assert.AreEqual(1.55555, eurUsdOrderBook.Items[0].Rate);
            Assert.AreEqual(BookOrderType.Bid, eurUsdOrderBook.Items[0].Type);
        }


    }
}
