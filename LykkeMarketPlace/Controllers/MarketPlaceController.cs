using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core;
using Core.Assets;
using Core.Finance;
using Core.Traders;
using LykkeMarketPlace.Models;

namespace LykkeMarketPlace.Controllers
{
    [Authorize]
    public class MarketPlaceController : Controller
    {
        private readonly ITradersRepository _tradersRepository;
        private readonly SrvBalanceAccess _srvBalanceAccess;
        private readonly IAssetsDictionary _assetsDictionary;
        private readonly IAssetPairsDictionary _assetPairsDictionary;


        public MarketPlaceController(ITradersRepository tradersRepository, SrvBalanceAccess srvBalanceAccess, 
            IAssetsDictionary assetsDictionary, IAssetPairsDictionary assetPairsDictionary)
        {
            _tradersRepository = tradersRepository;
            _srvBalanceAccess = srvBalanceAccess;
            _assetsDictionary = assetsDictionary;
            _assetPairsDictionary = assetPairsDictionary;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var id = this.GetTraderId();
            var viewModel = new MarketPlaceIndexViewModel
            {
                Assets = _assetsDictionary.GetAll().ToDictionary(itm => itm.Id),
                Trader = await _tradersRepository.GetByIdAsync(id),
                CurrencyBalances = await _srvBalanceAccess.GetCurrencyBalances(id)
            };

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult GetAssets(string asset)
        {

            var viewModel = new GetAssetsViewModel
            {
                Asset = asset,
                AssetPairs = _assetPairsDictionary.FindByBasedOrQuotingAsset(asset).ToArray()
            };

            return View(viewModel);
        }
    }
}