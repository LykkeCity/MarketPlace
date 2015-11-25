using System.Collections.Generic;

namespace Core.Assets
{
    public interface IAssetPairsDictionary
    {
        IEnumerable<IAssetPair> GetAll();
        IAssetPair Get(string id);

        IEnumerable<IAssetPair> FindByBasedOrQuotingAsset(string id);

        IAssetPair FindByBasedOrQuotingAsset(string asset1, string asset2);
    }
}
