using System.Threading.Tasks;
using Core.Accounts;
using Core.BitCoin;
using Core.Clients;

namespace LkeServices.Clients
{
    public class SrvClientManager
    {
        private readonly IClientAccountsRepository _tradersRepository;
        private readonly ISrvSmsConfirmator _srvSmsConfirmator;
        private readonly IPersonalDataRepository _personalDataRepository;
        private readonly ISrvLykkeWallet _srvLykkeWallet;
        private readonly IAccountsRepository _accountsRepository;

        public SrvClientManager(IClientAccountsRepository tradersRepository, ISrvSmsConfirmator srvSmsConfirmator, 
            IPersonalDataRepository personalDataRepository, ISrvLykkeWallet srvLykkeWallet, IAccountsRepository accountsRepository)
        {
            _tradersRepository = tradersRepository;
            _srvSmsConfirmator = srvSmsConfirmator;
            _personalDataRepository = personalDataRepository;
            _srvLykkeWallet = srvLykkeWallet;
            _accountsRepository = accountsRepository;
        }

        private async Task RegisterAccountAsync(IClientAccount client, string currencyId)
        {
            var lykkeAccount = await _srvLykkeWallet.GenerateAccountsAsync(currencyId);
            await _accountsRepository.RegisterAccount(Account.Create(client.Id, lykkeAccount.Id, 0, currencyId));
        }

        public async Task<IClientAccount> RegisterClientAsync(IClientAccount clientAccount, string password)
        {

            clientAccount = await _tradersRepository.RegisterAsync(clientAccount, password);
            await _srvSmsConfirmator.SendSmsAsync(clientAccount.Id);

            await _personalDataRepository.SaveAsync(PersonalData.Create(clientAccount));

            await RegisterAccountAsync(clientAccount, "EUR");
            await RegisterAccountAsync(clientAccount, "USD");
            await RegisterAccountAsync(clientAccount, "CHF");

            return clientAccount;

        }

    }
}
