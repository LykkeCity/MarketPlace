using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Orders;
using LkeServices.Orders;
using Tests.Env;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var ioc = MockEnvironmentCreator.Create();

            var trader = ioc.RegisterTrader("test@test.tt");

            var srvOrderRegistrator = ioc.GetObject<SrvOrdersRegistrator>();

            ioc.DepositAcount(trader.Id, "EUR", 10000);

            var limitOrder = LimitOrder.Create(trader.Id, "EURUSD", "USD", OrderAction.Sell, 1000, 1.55555);
            srvOrderRegistrator.RegisterTradeOrderAsync(limitOrder).Wait();

            var marketOrder = MarketOrder.Create(trader.Id, "EURUSD", "USD", OrderAction.Buy, 1000);
            srvOrderRegistrator.RegisterTradeOrderAsync(marketOrder).Wait();

            var eurBalance = ioc.GetBalance(trader.Id, "EUR");
            var usdBalance = ioc.GetBalance(trader.Id, "USD");

            Console.WriteLine("EUR Balance "+eurBalance);
            Console.WriteLine("USD Balance " + usdBalance);

            Console.ReadLine();

        }
    }
}
