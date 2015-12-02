using System.Threading.Tasks;
using Core.Clients;

namespace LkeServices.Clients
{
    public class SrvClientManager
    {
        private readonly IClientAccountsRepository _tradersRepository;
        private readonly ISrvSmsConfirmator _srvSmsConfirmator;
        private readonly IPersonalDataRepository _personalDataRepository;

        public SrvClientManager(IClientAccountsRepository tradersRepository, ISrvSmsConfirmator srvSmsConfirmator, IPersonalDataRepository personalDataRepository)
        {
            _tradersRepository = tradersRepository;
            _srvSmsConfirmator = srvSmsConfirmator;
            _personalDataRepository = personalDataRepository;
        }

        public async Task<IClientAccount> RegisterClientAsync(IClientAccount clientAccount, string password)
        {
            clientAccount = await _tradersRepository.RegisterAsync(clientAccount, password);
            await _srvSmsConfirmator.SendSmsAsync(clientAccount.Id);

            await _personalDataRepository.SaveAsync(PersonalData.Create(clientAccount));
            return clientAccount;
        }

    }
}
