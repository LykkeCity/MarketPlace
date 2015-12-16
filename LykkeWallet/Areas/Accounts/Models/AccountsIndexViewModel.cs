using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Accounts;

namespace LykkeWallet.Areas.Accounts.Models
{
    public class AccountsIndexViewModel
    {
        public IEnumerable<IAccount> Accounts { get; set; } 
    }
}