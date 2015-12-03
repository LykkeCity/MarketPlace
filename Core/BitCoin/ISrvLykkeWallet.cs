using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BitCoin
{
    public class LykkeAccount
    {
        public string Id { get; set; }
        public string Currency { get; set; }
        public double Balance { get; set; }
    }


    //ToDo - Replace Mock instance
    public interface ISrvLykkeWallet
    {
        Task<LykkeAccount> GenerateAccountsAsync(string currency);
        Task<LykkeAccount> GetAsync(string id);
        Task DepositWithdrawAsync(string id, double amount);
    }

    public class MyClass
    {
        public MyClass()
        {
            
        }

    }

}
