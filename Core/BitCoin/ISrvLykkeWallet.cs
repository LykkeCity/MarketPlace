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
        Task<LykkeAccount> DepositWithdrawAsync(string accountId, double amount);
    }


}
