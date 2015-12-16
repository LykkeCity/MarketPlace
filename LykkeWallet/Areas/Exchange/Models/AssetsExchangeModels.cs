using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Accounts;
using Core.Assets;

namespace LykkeWallet.Areas.Exchange.Models
{
    public class AssetsExchangeIndexViewModel
    {

        public IEnumerable<IAccount> Accounts { get; set; }
    }
}