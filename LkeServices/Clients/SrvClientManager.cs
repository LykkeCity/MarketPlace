using System.Threading.Tasks;
using Core.Accounts;
using Core.BitCoin;
using Core.Clients;
using Core.EventLogs;

namespace LkeServices.Clients
{
    public class SrvClientManager
    {
        private readonly IClientAccountsRepository _tradersRepository;
        private readonly ISrvSmsConfirmator _srvSmsConfirmator;
        private readonly IPersonalDataRepository _personalDataRepository;
        private readonly ISrvLykkeWallet _srvLykkeWallet;
        private readonly IAccountsRepository _accountsRepository;
        private readonly IRegistrationLogs _registrationLogs;
        private readonly IRegistrationConsumer[] _registrationConsumers;

        public SrvClientManager(IClientAccountsRepository tradersRepository, ISrvSmsConfirmator srvSmsConfirmator, 
            IPersonalDataRepository personalDataRepository, ISrvLykkeWallet srvLykkeWallet, IAccountsRepository accountsRepository, IRegistrationLogs registrationLogs,
            IRegistrationConsumer[] registrationConsumers)
        {
            _tradersRepository = tradersRepository;
            _srvSmsConfirmator = srvSmsConfirmator;
            _personalDataRepository = personalDataRepository;
            _srvLykkeWallet = srvLykkeWallet;
            _accountsRepository = accountsRepository;
            _registrationLogs = registrationLogs;
            _registrationConsumers = registrationConsumers;
        }

        private async Task RegisterAccountAsync(IClientAccount client, string currencyId)
        {
            var lykkeAccount = await _srvLykkeWallet.GenerateAccountsAsync(currencyId);
            await _accountsRepository.RegisterAccount(Account.Create(client.Id, lykkeAccount.Id, 0, currencyId));
        }


        public async Task<IClientAccount> RegisterClientAsync(string email, string fullname, string phone, string password, string clientInfo, string ip)
        {
            IClientAccount clientAccount = ClientAccount.Create(email, phone);

            clientAccount = await _tradersRepository.RegisterAsync(clientAccount, password);
            await _srvSmsConfirmator.SendSmsAsync(clientAccount.Id);

            await _personalDataRepository.SaveAsync(PersonalData.Create(clientAccount, fullname));

            await RegisterAccountAsync(clientAccount, "EUR");
            await RegisterAccountAsync(clientAccount, "USD");
            await RegisterAccountAsync(clientAccount, "CHF");

            var logEvent = RegistrationLogEvent.Create(clientAccount.Id, email, fullname, phone, clientInfo, ip);
            var regItem = await _registrationLogs.RegisterEventAsync(logEvent);

            foreach (var registrationConsumer in _registrationConsumers)
                await registrationConsumer.ConsumeRegistration(clientAccount, ip, regItem.Id);

            return clientAccount;

        }

    }
}
