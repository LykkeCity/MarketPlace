using System.Web.Http;
using Wallet_Api.Models;

namespace Wallet_Api.Controllers
{
    [Authorize]
    public class LykkeWalletController : ApiController
    {

        private readonly ApiWalletAssetModel[] _mockLykkeWalletData = 
        {
            new ApiWalletAssetModel
            {
                Id = "USD",
                Name = "ccUSD",
                Balance = 0,
                Symbol = "$"
            },

            new ApiWalletAssetModel
            {
                Id = "EUR",
                Name = "ccEUR",
                Balance = 0,
                Symbol = "€"
            },

            new ApiWalletAssetModel
            {
                Id = "CHF",
                Name = "ccCHF",
                Balance = 0,
                Symbol = "f"
            }
        };


        private readonly ApiBankCardModel[] _mockBankCards =
        {
            new ApiBankCardModel
            {
                Id = "aaaaa",
                LastDigits = "1798",
                Type = 'V'
            },

            new ApiBankCardModel
            {
                Id = "bbbbb",
                LastDigits = "0295",
                Type = 'M'
            }
        };


        public ResponseModel<GetWaletRespModel> Get()
        {
            return ResponseModel<GetWaletRespModel>.CreateOk(
                new GetWaletRespModel
                {
                    Lykke = new GetWaletRespModel.LykkeWalletsModel
                    {
                        Assets = _mockLykkeWalletData,
                        Equity = 0
                    },

                    BankCards = _mockBankCards



                });
        }

    }
}
