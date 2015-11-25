
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Assets;
using Core.Orders;
using Core.Traders;
using LykkeMarketPlace.Models;

namespace LykkeMarketPlace.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly ITraderSettingsRepository _traderSettingsRepository;
        private readonly IAssetPairsDictionary _assetPairsDictionary;

        public OrdersController(IOrdersRepository ordersRepository, ITraderSettingsRepository traderSettingsRepository, IAssetPairsDictionary assetPairsDictionary)
        {
            _ordersRepository = ordersRepository;
            _traderSettingsRepository = traderSettingsRepository;
            _assetPairsDictionary = assetPairsDictionary;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var traderId = this.GetTraderId();

            var viewModel = new OrdersIndexViewModel
            {
                OrdersRequestSettings = await _traderSettingsRepository.GetSettings<OrdersRequestSettings>(traderId)
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
            await _traderSettingsRepository.SetSettings(traderId, orderSettings);

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