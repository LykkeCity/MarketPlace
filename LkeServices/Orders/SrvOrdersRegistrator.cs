using System;
using System.Threading.Tasks;
using Core;
using Core.Orders;

namespace LkeServices.Orders
{

    public class SrvOrdersRegistrator
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IIdentityGenerator _identityGenerator;
        private readonly IOrderExecuter _orderExecuter;

        public SrvOrdersRegistrator(IOrdersRepository ordersRepository, IIdentityGenerator identityGenerator, IOrderExecuter orderExecuter)
        {
            _ordersRepository = ordersRepository;
            _identityGenerator = identityGenerator;
            _orderExecuter = orderExecuter;
        }

        private async Task HandleCommonStuff(OrderBase order)
        {
            var id = await _identityGenerator.GenerateNewIdAsync();
            order.Id = id;
            order.Status = OrderStatus.Registered;
            order.Registered = DateTime.UtcNow;
        }

        public async Task RegisterTradeOrderAsync(TraderOrderBase order)
        {
            await HandleCommonStuff(order);
            await _ordersRepository.RegisterOrderAsync(order);
            await _orderExecuter.Execute();
        }


        public async Task RegisterLinkedLimitOrders(LimitOrder order1, LimitOrder order2)
        {
            await HandleCommonStuff(order1);
            await HandleCommonStuff(order2);

            order2.LinkedLimitOrder = order1.Id;
            order1.LinkedLimitOrder = order2.Id;

            await _ordersRepository.RegisterOrderAsync(order1);
            await _ordersRepository.RegisterOrderAsync(order2);
            await _orderExecuter.Execute();

        }



    }
}
