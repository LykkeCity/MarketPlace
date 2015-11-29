using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Clients;

namespace LkeServices.Clients
{
    public class SrvClientManager
    {
        private readonly IClientAccountsRepository _tradersRepository;
        private readonly ISrvSmsConfirmator _srvSmsConfirmator;

        public SrvClientManager(IClientAccountsRepository tradersRepository, ISrvSmsConfirmator srvSmsConfirmator)
        {
            _tradersRepository = tradersRepository;
            _srvSmsConfirmator = srvSmsConfirmator;
        }


        public async Task<IClientAccount> RegisterClientAsync(IClientAccount clientAccount, string password)
        {
            clientAccount = await _tradersRepository.RegisterAsync(clientAccount, password);
            await _srvSmsConfirmator.SendSmsAsync(clientAccount.Id);
            return clientAccount;
        }

    }
}
