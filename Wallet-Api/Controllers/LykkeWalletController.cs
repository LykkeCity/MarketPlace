using System.Web.Http;
using Wallet_Api.Models;

namespace Wallet_Api.Controllers
{
    [Authorize]
    public class LykkeWalletController : ApiController
    {

        public ResponseModel<WalletResponseModel> Get()
        {
            return ResponseModel<WalletResponseModel>.CreateOk(
                new WalletResponseModel
                {
                    Equity = 0,
                    Assets = new[]
                    {
                        new WalletAsset
                        {
                            Id = "USD",
                            Name = "ccUSD",
                            Balance = 0,
                            Symbol = "$"
                        },

                        new WalletAsset
                        {
                            Id = "EUR",
                            Name = "ccEUR",
                            Balance = 0,
                            Symbol = "€"
                        },

                        new WalletAsset
                        {
                            Id = "CHF",
                            Name = "ccCHF",
                            Balance = 0,
                            Symbol = "f"
                        }
                    }
                });
        }
    }
}
