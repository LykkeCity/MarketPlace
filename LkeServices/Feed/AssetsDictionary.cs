using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Core.Assets;

namespace LkeServices.Feed
{
    public class AssetDictionary : TimerPeriod, IAssetsDictionary
    {
        private readonly IAssetsRepository _assetsRepository;
        private Dictionary<string, IAsset> _cache = new Dictionary<string, IAsset>(); 


        public AssetDictionary(IAssetsRepository assetsRepository, ILog log) : base("AssetCachedReader", 5*60000, log)
        {
            _assetsRepository = assetsRepository;
        }

        private Dictionary<string, IAsset> GetCache()
        {
            if (_cache == null)
                LoadCache().Wait();

            return _cache;

        }

        public IEnumerable<IAsset> GetAll()
        {
            var localCache = GetCache();
            return localCache.Values.ToArray();
        }

        public IAsset Find(string id)
        {
            var localCache = GetCache();
           return localCache.ContainsKey(id) ? localCache[id] : null;
        }

        private async Task LoadCache()
        {
            var assets = await _assetsRepository.GetAssetsAsync();
            _cache = assets.ToDictionary(itm => itm.Id);
        }


        protected override Task Execute()
        {
            return LoadCache();
        }
    }
}
