
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Assets;
using Core.Clients;
using Core.Orders;
using LykkeMarketPlace.Models;

namespace LykkeMarketPlace.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IClientSettingsRepository _clientSettingsRepository;
        private readonly IAssetPairsDictionary _assetPairsDictionary;

        public OrdersController(IOrdersRepository ordersRepository, IClientSettingsRepository clientSettingsRepository, IAssetPairsDictionary assetPairsDictionary)
        {
            _ordersRepository = ordersRepository;
            _clientSettingsRepository = clientSettingsRepository;
            _assetPairsDictionary = assetPairsDictionary;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var traderId = this.GetTraderId();

            var viewModel = new OrdersIndexViewModel
            {
                OrdersRequestSettings = await _clientSettingsRepository.GetSettings<OrdersRequestSettings>(traderId)
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Get(GetOrdersModel data)
        {
            var traderId = this.GetTraderId();


            var orderSettings = new OrdersRequestSettings
            {
                ActiveChecked = data.Active != null,
                DoneChecked = data.Done != null,
                CanceledChecked = data.Canceled != null,
            };
            await _clientSettingsRepository.SetSettings(traderId, orderSettings);

            var orders = await _ordersRepository.GetOrdersByTraderAsync(traderId, o =>
                (data.Active != null && o.Status == OrderStatus.Registered) |
                (data.Canceled != null && o.Status == OrderStatus.Canceled) |
                (data.Done != null && o.Status == OrderStatus.Done));

            var viewModel = new GetOrdersViewModel
            {
                AssetPairs = _assetPairsDictionary.GetAll().ToDictionary(itm => itm.Id),
                Orders = orders.ToArray()
            };

            return View(viewModel);
        }
   
 }
}