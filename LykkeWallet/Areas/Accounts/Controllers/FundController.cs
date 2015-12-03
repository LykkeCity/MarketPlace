using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Core.Accounts;
using Core.BitCoin;
using LykkeWallet.Areas.Accounts.Models;
using LykkeWallet.Controllers;
using LykkeWallet.Strings;

namespace LykkeWallet.Areas.Accounts.Controllers
{
    [Authorize]
    public class FundController : Controller
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ISrvLykkeWallet _srvLykkeWallet;

        public FundController(IAccountsRepository accountsRepository, ISrvLykkeWallet srvLykkeWallet)
        {
            _accountsRepository = accountsRepository;
            _srvLykkeWallet = srvLykkeWallet;
        }


        [HttpPost]
        public async Task<ActionResult> Details(string id)
        {
            var clientId = this.GetClientId();

            var viewModel = new DepositAccountModels
            {
                Account = await _accountsRepository.GetAccountAsync(clientId, id)
            };

            return View(viewModel);
        }


        private bool CheckBankAcccount(string bankAccoung)
        {
            bankAccoung = bankAccoung.Replace(" ", "");

            if (bankAccoung.Length != 16)
                return false;

            if (!bankAccoung.IsOnlyDigits())
                return false;

            return true;

        }

        [HttpPost]
        public async Task<ActionResult> Do(FundAccountByBankCardModel model)
        {
            if (string.IsNullOrEmpty(model.CardOwner))
                return this.JsonFailResult("#cardOwner", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(model.CardNumber))
                return this.JsonFailResult("#cardNumber", Phrases.FieldShouldNotBeEmpty);

            if (!CheckBankAcccount(model.CardNumber))
                return this.JsonFailResult("#cardNumber", Phrases.InvalidCardNumberFormat);

            if (string.IsNullOrEmpty(model.CardCcv))
                return this.JsonFailResult("#cardCcv", Phrases.FieldShouldNotBeEmpty);

            if (string.IsNullOrEmpty(model.Amount))
                return this.JsonFailResult("#amount", Phrases.FieldShouldNotBeEmpty);

            double amount;
            try
            {
                amount = model.Amount.ParseAnyDouble();
            }
            catch (Exception)
            {
                return this.JsonFailResult("#amount", Phrases.InvalidAmountFormat);
            }

            var account = await _srvLykkeWallet.DepositWithdrawAsync(model.AccountId, amount);

            var clientId = this.GetClientId();
            await _accountsRepository.UpdateBalanceAsync(clientId, account.Id, account.Balance);

            return this.JsonShowContentResult("#pamain", Url.Action("Index","List"), null,new JsonResultExtParams {HideDetails = true});

        }
    }
}