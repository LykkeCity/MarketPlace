using System;
using Core.Orders;
using LkeServices.Orders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Env;

namespace Tests
{
    [TestClass]
    public class OrderExecutionTests
    {

        [TestMethod]
        public void Text_Exchange1()
        {
            var ioc = MockEnvironmentCreator.Create();

            var trader1 = ioc.RegisterTrader("test@test.tt");
            var trader2 = ioc.RegisterTrader("test2@test.tt");

            var srvOrderRegistrator = ioc.GetObject<SrvOrdersRegistrator>();

            ioc.DepositAcount(trader1.Id, "USD", 10000);
            ioc.DepositAcount(trader2.Id, "EUR", 10000);

            var limitOrder = LimitOrder.Create(trader1.Id, "EURUSD", "USD", OrderAction.Sell, 1000, 2);
            srvOrderRegistrator.RegisterTradeOrderAsync(limitOrder).Wait();

            var marketOrder = MarketOrder.Create(trader2.Id, "EURUSD", "USD", OrderAction.Buy, 1000);
            srvOrderRegistrator.RegisterTradeOrderAsync(marketOrder).Wait();

            var eurBalanceTrader1 = ioc.GetBalance(trader1.Id, "EUR");
            var usdBalanceTrader1 = ioc.GetBalance(trader1.Id, "USD");

            var eurBalanceTrader2 = ioc.GetBalance(trader2.Id, "EUR");
            var usdBalanceTrader2 = ioc.GetBalance(trader2.Id, "USD");

            Assert.AreEqual(9000, Math.Round(usdBalanceTrader1, 2));
            Assert.AreEqual(500, Math.Round(eurBalanceTrader1, 2));

            Assert.AreEqual(1000, Math.Round(usdBalanceTrader2, 2));
            Assert.AreEqual(9500, Math.Round(eurBalanceTrader2, 2));
        }


        [TestMethod]
        public void Text_Exchange_NotEnoughFunds()
        {
            var ioc = MockEnvironmentCreator.Create();

            var trader1 = ioc.RegisterTrader("test@test.tt");
            var trader2 = ioc.RegisterTrader("test2@test.tt");

            var srvOrderRegistrator = ioc.GetObject<SrvOrdersRegistrator>();

            ioc.DepositAcount(trader1.Id, "USD", 500);
            ioc.DepositAcount(trader2.Id, "EUR", 10000);

            var limitOrder = LimitOrder.Create(trader1.Id, "EURUSD", "USD", OrderAction.Sell, 1000, 1.55555);
            srvOrderRegistrator.RegisterTradeOrderAsync(limitOrder).Wait();

            var marketOrder = MarketOrder.Create(trader2.Id, "EURUSD", "USD", OrderAction.Buy, 1000);
            srvOrderRegistrator.RegisterTradeOrderAsync(marketOrder).Wait();

            var eurBalanceTrader1 = ioc.GetBalance(trader1.Id, "EUR");
            var usdBalanceTrader1 = ioc.GetBalance(trader1.Id, "USD");

            var eurBalanceTrader2 = ioc.GetBalance(trader2.Id, "EUR");
            var usdBalanceTrader2 = ioc.GetBalance(trader2.Id, "USD");

            Assert.AreEqual(500, Math.Round(usdBalanceTrader1, 2));
            Assert.AreEqual(0, Math.Round(eurBalanceTrader1, 2));


            Assert.AreEqual(0, Math.Round(usdBalanceTrader2, 2));
            Assert.AreEqual(10000, Math.Round(eurBalanceTrader2, 2));
        }


        [TestMethod]
        public void Text_Exchange_USDJPY()
        {
            var ioc = MockEnvironmentCreator.Create();

            var trader1 = ioc.RegisterTrader("test@test.tt");
            var trader2 = ioc.RegisterTrader("test2@test.tt");

            var srvOrderRegistrator = ioc.GetObject<SrvOrdersRegistrator>();

            ioc.DepositAcount(trader1.Id, "USD", 10000);
            ioc.DepositAcount(trader2.Id, "EUR", 10000);

            var limitOrder = LimitOrder.Create(trader1.Id, "EURUSD", "USD", OrderAction.Sell, 1000, 2);
            srvOrderRegistrator.RegisterTradeOrderAsync(limitOrder).Wait();

            var marketOrder = MarketOrder.Create(trader2.Id, "EURUSD", "USD", OrderAction.Buy, 1000);
            srvOrderRegistrator.RegisterTradeOrderAsync(marketOrder).Wait();

            var eurBalanceTrader1 = ioc.GetBalance(trader1.Id, "EUR");
            var usdBalanceTrader1 = ioc.GetBalance(trader1.Id, "USD");

            var eurBalanceTrader2 = ioc.GetBalance(trader2.Id, "EUR");
            var usdBalanceTrader2 = ioc.GetBalance(trader2.Id, "USD");

            Assert.AreEqual(9000, Math.Round(usdBalanceTrader1, 2));
            Assert.AreEqual(500, Math.Round(eurBalanceTrader1, 2));


            Assert.AreEqual(1000, Math.Round(usdBalanceTrader2, 2));
            Assert.AreEqual(9500, Math.Round(eurBalanceTrader2, 2));
        }

        [TestMethod]
        public void Test_2LimitsAutoexecute()
        {
            var ioc = MockEnvironmentCreator.Create();

            var trader1 = ioc.RegisterTrader("test@test.tt");
            var trader2 = ioc.RegisterTrader("test2@test.tt");

            var srvOrderRegistrator = ioc.GetObject<SrvOrdersRegistrator>();

            ioc.DepositAcount(trader1.Id, "USD", 10000);
            ioc.DepositAcount(trader2.Id, "EUR", 10000);

            var limitOrder = LimitOrder.Create(trader1.Id, "EURUSD", "USD", OrderAction.Sell, 1000, 2);
            srvOrderRegistrator.RegisterTradeOrderAsync(limitOrder).Wait();

            var marketOrder = LimitOrder.Create(trader2.Id, "EURUSD", "USD", OrderAction.Buy, 1000, 2);
            srvOrderRegistrator.RegisterTradeOrderAsync(marketOrder).Wait();

            var eurBalanceTrader1 = ioc.GetBalance(trader1.Id, "EUR");
            var usdBalanceTrader1 = ioc.GetBalance(trader1.Id, "USD");

            var eurBalanceTrader2 = ioc.GetBalance(trader2.Id, "EUR");
            var usdBalanceTrader2 = ioc.GetBalance(trader2.Id, "USD");

            Assert.AreEqual(9000, Math.Round(usdBalanceTrader1, 2));
            Assert.AreEqual(500, Math.Round(eurBalanceTrader1, 2));


            Assert.AreEqual(1000, Math.Round(usdBalanceTrader2, 2));
            Assert.AreEqual(9500, Math.Round(eurBalanceTrader2, 2));
        }


    }

}
