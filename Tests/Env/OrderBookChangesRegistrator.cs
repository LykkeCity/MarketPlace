using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Orders;

namespace Tests.Env
{
    public class OrderBookChangesRegistrator : IOrderBookChangesConsumer
    {
        public Task OrderBookChanged(string asset)
        {
            Changes.Add(asset);
            return Task.FromResult(0);
        }

        public readonly List<string> Changes = new List<string>(); 

    }

}
