using System.Collections.Generic;

namespace Core.Assets
{
    public interface IAssetsDictionary
    {
        IEnumerable<IAsset> GetAll();
        IAsset Find(string id);
    }

}
