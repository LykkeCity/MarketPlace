using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Core.Orders;

namespace LykkeMarketPlace.Services.OrderBook
{
    public class UpdateOrderBookConsumer : IOrderBookChangesConsumer
    {
        private readonly Func<string, Task> _notify;

        public UpdateOrderBookConsumer(Func<string,Task> notify)
        {
            _notify = notify;
        }


        public Task OrderBookChanged(string asset)
        {
            return _notify(asset);
        }
    }
}