using Core.Accounts;

namespace LykkeWallet.Areas.Accounts.Models
{
    public class DepositAccountModels
    {
        public IAccount Account { get; set; } 
    }

    public class FundAccountByBankCardModel
    {
        public string CardNumber { get; set; }
        public string CardOwner { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public string CardCcv { get; set; }
        public string Amount { get; set; }
        public string AccountId { get; set; }
    }
}