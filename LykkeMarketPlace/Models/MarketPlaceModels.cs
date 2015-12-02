using System.Collections.Generic;
using Core.Assets;
using Core.Clients;
using Core.Finance;

namespace LykkeMarketPlace.Models
{
    public class MarketPlaceIndexViewModel
    {
        public Dictionary<string, IAsset> Assets { get; set; } 

        public IClientAccount Trader { get; set; }
        public CurrencyBalance[] CurrencyBalances { get; set; }
    }

    public class GetAssetsViewModel
    {
        public string Asset { get; set; }
        public IAssetPair[] AssetPairs { get; set; } 
    }

}