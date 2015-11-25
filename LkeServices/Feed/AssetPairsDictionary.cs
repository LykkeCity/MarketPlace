using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Core.Assets;

namespace LkeServices.Feed
{
    public class AssetPairsDictionary : TimerPeriod, IAssetPairsDictionary
    {
        private readonly IAssetPairsRepository _assetPairsRepository;

        private Dictionary<string, IAssetPair> _cache;

        public AssetPairsDictionary(IAssetPairsRepository assetPairsRepository, ILog log)
            : base("AssetPairsDictionary", 5*60000, log)
        {
            _assetPairsRepository = assetPairsRepository;
        }


        private Dictionary<string, IAssetPair> GetCache()
        {
            if (_cache == null)
                LoadCache().Wait();

            return _cache;
        }


        public IEnumerable<IAssetPair> GetAll()
        {
            return GetCache().Values.ToArray();
        }

        public IAssetPair Get(string id)
        {
            var localCache = GetCache();
            return localCache.ContainsKey(id) ? localCache[id] : null;
        }

        public IEnumerable<IAssetPair> FindByBasedOrQuotingAsset(string id)
        {
            var localCache = GetCache();
            return localCache.Values.Where(itm => itm.BaseAssetId == id || itm.QuotingAssetId == id).ToArray();
        }

        public IAssetPair FindByBasedOrQuotingAsset(string asset1, string asset2)
        {
            var localCache = GetCache();
            foreach (var fi in localCache.Values)
            {
                if (fi.BaseAssetId == asset1 && fi.QuotingAssetId == asset2)
                    return fi;

                if (fi.QuotingAssetId == asset1 && fi.BaseAssetId == asset2)
                    return fi;
            }

            return null;
        }



        private async Task LoadCache()
        {
            var assetPairs = await _assetPairsRepository.GetAllAsync();
            _cache = assetPairs.ToDictionary(itm => itm.Id);

        }


        protected override Task Execute()
        {
            return LoadCache();
        }
    }
}
