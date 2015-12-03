using System;
using System.Threading.Tasks;
using Core.BitCoin;

namespace MockServices.BitCoin
{
    public class SrvLykkeWalletMock : ISrvLykkeWallet
    {
        private readonly IMockLykkeWalletRepository _mockLykkeWalletRepository;

        public SrvLykkeWalletMock(IMockLykkeWalletRepository mockLykkeWalletRepository)
        {
            _mockLykkeWalletRepository = mockLykkeWalletRepository;
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

        public Task DepositWithdrawAsync(string accountid, double amount)
        {
            return _mockLykkeWalletRepository.DepositWithdrawAsync(accountid, amount);
        }
    }
}
