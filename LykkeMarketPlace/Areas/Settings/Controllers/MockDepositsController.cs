using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Core.Assets;
using Core.Finance;
using LykkeMarketPlace.Areas.Settings.Models;
using LykkeMarketPlace.Controllers;
using LykkeMarketPlace.Hubs;
using LykkeMarketPlace.Strings;

namespace LykkeMarketPlace.Areas.Settings.Controllers
{
    [Authorize]
    public class MockDepositsController : Controller
    {
        private readonly SrvBalanceAccess _srvBalanceAccess;
        private readonly IAssetsDictionary _assetsDictionary;

        public MockDepositsController(SrvBalanceAccess srvBalanceAccess, IAssetsDictionary assetsDictionary)
        {
            _srvBalanceAccess = srvBalanceAccess;
            _assetsDictionary = assetsDictionary;
        }

        [HttpPost]
        public ActionResult Index()
        {
            var viewModel = new MockDepositIndexViewModel
            {
                Assets = _assetsDictionary.GetAll()
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> MockDeposit(MockDepositModel model)
        {
            if (string.IsNullOrEmpty(model.Amount))
                return this.JsonFailResult("#amount", Phrases.FieldShouldNotBeEmpty);

            double amount = 0;

            try
            {
                amount = model.Amount.ParseAnyDouble();
            }
            catch (Exception)
            {
                return this.JsonFailResult("#amount", Phrases.InvalidAmountFormat);
            }

            var id = this.GetTraderId();

            await _srvBalanceAccess.ChangeBalance(id, model.Currency, amount);

            await LkHub.RefreshBalance(id);

            return this.JsonHideDialog();
        }
    }
}