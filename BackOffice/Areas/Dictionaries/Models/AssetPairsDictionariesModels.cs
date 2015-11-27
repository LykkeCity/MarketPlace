using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Assets;

namespace BackOffice.Areas.Dictionaries.Models
{
    public class AssetPairsIndexViewModel
    {
        public IEnumerable<IAssetPair> AssetPairs { get; set; } 
        
    }
}