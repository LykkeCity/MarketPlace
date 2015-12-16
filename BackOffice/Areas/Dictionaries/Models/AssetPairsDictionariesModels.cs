using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackOffice.Models;
using Core.Assets;

namespace BackOffice.Areas.Dictionaries.Models
{
    public class AssetPairsIndexViewModel
    {
        public Dictionary<string, IAsset> Assets { get; set; }
        public IEnumerable<IAssetPair> AssetPairs { get; set; } 
    }


    public class AssetPairsEditViewModel : IPersonalAreaDialog
    {
        public string Caption { get; set; }
        public string Width { get; set; }
        public Dictionary<string, IAsset> Assets { get; set; } 
        public IAssetPair AssetPair { get; set; }
    }


    public class AssetPairEditModel : IAssetPair
    {
        public string EditId { get; set; }
        public string Id { get; set; }
        public string BaseAssetId { get; set; }
        public string QuotingAssetId { get; set; }
        public int Accuracy { get; set; }
    }

}