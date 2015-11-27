using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Dictionaries.Models;
using BackOffice.Controllers;
using BackOffice.Models;
using BackOffice.Translates;
using Core.Assets;

namespace BackOffice.Areas.Dictionaries.Controllers
{
    [Authorize]
    public class AssetPairsController : Controller
    {
        private readonly IAssetsRepository _assetsRepository;
        private readonly IAssetPairsRepository _assetPairsRepository;

        public AssetPairsController(IAssetsRepository assetsRepository, 
            IAssetPairsRepository assetPairsRepository)
        {
            _assetsRepository = assetsRepository;
            _assetPairsRepository = assetPairsRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var viewModel = new AssetPairsIndexViewModel
            {
                Assets = (await _assetsRepository.GetAssetsAsync()).ToDictionary(itm => itm.Id),
                AssetPairs = await _assetPairsRepository.GetAllAsync()
            };
            return View(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> EditDialog(string id)
        {
            var viewModel = new AssetPairsEditViewModel
            {
                Caption = Phrases.EditAssetPair,
                Assets = (await _assetsRepository.GetAssetsAsync()).ToDictionary(itm => itm.Id),
                AssetPair = string.IsNullOrEmpty(id) ? AssetPair.CreateDefault() : await _assetPairsRepository.GetAsync(id)
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(AssetPairEditModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#id");

            if (string.IsNullOrEmpty(model.EditId) || model.EditId != model.Id)
            {
                if (await _assetPairsRepository.GetAsync(model.Id) != null)
                    return this.JsonFailResult(Phrases.AssetPairWithSameIdExists, "#id");
            }

            if (string.IsNullOrEmpty(model.BaseAssetId))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#baseAssetId");

            if (string.IsNullOrEmpty(model.QuotingAssetId))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#quotingAssetId");

            if (model.Accuracy <=0 )
                return this.JsonFailResult(Phrases.PleaseTypeYourNewPassword, "#accuracy");


            if (string.IsNullOrEmpty(model.EditId))
                await _assetPairsRepository.AddAsync(model);
            else
                await _assetPairsRepository.EditAsync(model.EditId, model);

            return this.JsonRequestResult(WebSiteConstants.PersonalAreaDiv, Url.Action("Index"));
        }

    }

}