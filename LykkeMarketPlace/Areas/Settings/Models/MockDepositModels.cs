using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Assets;

namespace LykkeMarketPlace.Areas.Settings.Models
{
    public class MockDepositIndexViewModel
    {
        public IEnumerable<IAsset> Assets { get; set; } 
    }

    public class MockDepositModel
    {
        public string Amount { get; set; }
        public string Currency { get; set; }
    }
}