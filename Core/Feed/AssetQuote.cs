using System;
using System.Threading.Tasks;

namespace Core.Feed
{

    public class AssetPairBestRate
    {
        public string Id { get; set; }
        public DateTime DateTime { get; set; }
        public double Bid { get; set; }
        public double Ask { get; set; }
    }

    public interface IAssetPairBestPriceNotifier
    {
        Task NotifyNewAsset(AssetPairBestRate assetQuote);
    }

}
