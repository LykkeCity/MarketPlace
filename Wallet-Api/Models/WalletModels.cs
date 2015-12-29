
namespace Wallet_Api.Models
{


    public class GetWaletRespModel
    {
        public class LykkeWalletsModel
        {
            public ApiWalletAssetModel[] Assets { get; set; }
            public double Equity { get; set; }
        }

        public LykkeWalletsModel Lykke { get; set; }
        public ApiBankCardModel[] BankCards { get; set; }
    }

}