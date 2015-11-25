using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Assets
{
    public interface IAssetPair
    {
        string Id { get; }
        string BaseAssetId { get; }
        string QuotingAssetId { get; }
        int Accuracy { get; }
    }

    public class AssetPair : IAssetPair
    {
        public string Id { get; set; }
        public string BaseAssetId { get; set; }
        public string QuotingAssetId { get; set; }
        public int Accuracy { get; set; }
    }

    public interface IAssetPairsRepository
    {
        Task<IEnumerable<IAssetPair>> GetAllAsync();
        Task SaveAsync(IAssetPair assetPair);
    }


    public static class AssetPairExt
    {
        public static int Multiplier(this IAssetPair assetPair)
        {
            return (int)Math.Pow(assetPair.Accuracy, 10);
        }

        public static string RateToString(this double src, IAssetPair assetPair)
        {
            var mask = "0." + new string('#', assetPair.Accuracy);
            return src.ToString(mask);
        }
    }

}
