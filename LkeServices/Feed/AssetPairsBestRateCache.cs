using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Core.Feed;

namespace LkeServices.Feed
{
    public class AssetPairsBestRateCache : IAssetPairBestPriceNotifier
    {

        private readonly Dictionary<string, AssetPairBestRate> _profile = new Dictionary<string, AssetPairBestRate>();

        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();

        public AssetPairBestRate GetPrice(string id)
        {
            _readerWriterLockSlim.EnterReadLock();
            try
            {
                return _profile.ContainsKey(id) ? _profile[id] : null;
            }
            finally
            {
                _readerWriterLockSlim.ExitReadLock();
            }

        }

        public Task NotifyNewAsset(AssetPairBestRate assetQuote)
        {
            _readerWriterLockSlim.EnterWriteLock();
            try
            {
                if (!_profile.ContainsKey(assetQuote.Id))
                    _profile.Add(assetQuote.Id, assetQuote);
                else
                    _profile[assetQuote.Id] = assetQuote;
            }
            finally
            {
                _readerWriterLockSlim.ExitWriteLock();
            }

            return Task.FromResult(0);
        }
    }
}
