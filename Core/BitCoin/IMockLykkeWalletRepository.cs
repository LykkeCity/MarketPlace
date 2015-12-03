using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.BitCoin
{
    public interface IMockLykkeWalletRepository
    {

        Task SaveAsync(LykkeAccount account);
        Task<LykkeAccount> GetAsync(string accountId);
        Task<LykkeAccount> DepositWithdrawAsync(string accountId, double amount);
    }


}
