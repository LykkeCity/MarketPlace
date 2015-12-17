
namespace Wallet_Api.Models
{
    public class WalletAsset
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
        public string Symbol { get; set; }
    }

    public class WalletResponseModel
    {
        public WalletAsset[] Assets { get; set; }
        public double Equity { get; set; }
    }
}