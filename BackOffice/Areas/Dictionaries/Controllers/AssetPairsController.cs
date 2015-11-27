using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BackOffice.Areas.Dictionaries.Models;
using Common;
using Core.Assets;

namespace BackOffice.Areas.Dictionaries.Controllers
{
    [Authorize]
    public class AssetPairsController : Controller
    {
        private readonly IAssetPairsRepository _assetPairsRepository;

        public AssetPairsController(IAssetPairsRepository assetPairsRepository)
        {
            _assetPairsRepository = assetPairsRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var viewModel = new AssetPairsIndexViewModel
            {
                AssetPairs = await _assetPairsRepository.GetAllAsync()
            };
            return View(viewModel);
        }
    }
}