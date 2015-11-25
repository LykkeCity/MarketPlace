using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Feed;

namespace Tests.Env
{
    public class AssetPairBestPricesChangesRegistrator : IAssetPairBestPriceNotifier
    {
        public Task NotifyNewAsset(AssetPairBestRate assetQuote)
        {
            Changes.Add(assetQuote);
            return Task.FromResult(0);
        }


        public readonly List<AssetPairBestRate> Changes = new List<AssetPairBestRate>(); 
    }
}
