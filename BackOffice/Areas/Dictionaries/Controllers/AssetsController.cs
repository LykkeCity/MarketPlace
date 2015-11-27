using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Dictionaries.Models;
using BackOffice.Controllers;
using BackOffice.Translates;
using Core.Assets;

namespace BackOffice.Areas.Dictionaries.Controllers
{
    [Authorize]
    public class AssetsController : Controller
    {
        private readonly IAssetsRepository _assetsRepository;

        public AssetsController(IAssetsRepository assetsRepository)
        {
            _assetsRepository = assetsRepository;
        }


        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var viewModel = new AssetsIndexViewModel
            {
                Assets = await _assetsRepository.GetAssetsAsync()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> EditAssetDialog(string id)
        {
            var viewModel = new EditAssetDialogViewModel
            {
                Caption = Phrases.EditAsset,
                Asset = string.IsNullOrEmpty(id) ? Asset.CreateDefault() : await _assetsRepository.GetAssetAsync(id)
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> EditAsset(EditAssetModel model)
        {

            if (string.IsNullOrEmpty(model.Id))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#id");

            if (string.IsNullOrEmpty(model.Name))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#name");

            if (string.IsNullOrEmpty(model.EditId))
                await _assetsRepository.RegisterAssetAsync(model);
            else
            {
                if (model.Id == model.EditId)
                    if (await _assetsRepository.GetAssetAsync(model.Id) != null)
                        return this.JsonFailResult(Phrases.AssetWithSameIdExists, "#id");

                await _assetsRepository.EditAssetAsync(model.EditId, model);
            }


            return this.JsonRequestResult("#pamain", Url.Action("Index"));
        }

    }
}