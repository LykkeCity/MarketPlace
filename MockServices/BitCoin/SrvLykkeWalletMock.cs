using System;
using System.Threading.Tasks;
using Core.Accounts;
using Core.BitCoin;

namespace MockServices.BitCoin
{
    public class SrvLykkeWalletMock : ISrvLykkeWallet
    {
        private readonly IMockLykkeWalletRepository _mockLykkeWalletRepository;
        private readonly IAccountsRepository _accountsRepository;

        public SrvLykkeWalletMock(IMockLykkeWalletRepository mockLykkeWalletRepository, IAccountsRepository accountsRepository)
        {
            _mockLykkeWalletRepository = mockLykkeWalletRepository;
            _accountsRepository = accountsRepository;
        }

        public async Task<LykkeAccount> GenerateAccountsAsync(string currency)
        {
            //ToDo - Think later about Network Errors

            var result = new LykkeAccount
            {
                Id = Guid.NewGuid().ToString("N"),
                Currency = currency,
                Balance = 0
            };

            await _mockLykkeWalletRepository.SaveAsync(result);

            return result;
        }

        public Task<LykkeAccount> GetAsync(string accountid)
        {
            return _mockLykkeWalletRepository.GetAsync(accountid);
        }

        public Task<LykkeAccount> DepositWithdrawAsync(string accountId, double amount)
        {
            return _mockLykkeWalletRepository.DepositWithdrawAsync(accountId, amount);
        }

    }
}
