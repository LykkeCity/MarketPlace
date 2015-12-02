using System.Threading.Tasks;
using Common;
using Core.Clients;

namespace LkeServices.Clients
{
    public class SrvClientFinder
    {
        private readonly IPersonalDataRepository _personalDataRepository;
        private readonly IClientAccountsRepository _clientAccountsRepository;

        public SrvClientFinder(IPersonalDataRepository personalDataRepository, 
            IClientAccountsRepository clientAccountsRepository)
        {
            _personalDataRepository = personalDataRepository;
            _clientAccountsRepository = clientAccountsRepository;
        }

        public async Task<IPersonalData> FindClientAsync(string phrase)
        {
            if (string.IsNullOrEmpty(phrase))
                return null;

            if (phrase.IsGuid())
                return await _personalDataRepository.GetAsync(phrase);

            if (phrase.IsValidEmail())
            {
                var client = await _clientAccountsRepository.GetByEmailAsync(phrase);
                if (client == null)
                    return null;

                return await _personalDataRepository.GetAsync(client.Id);
            }

            return await _personalDataRepository.ScanAndFindAsync(itm =>
                (!string.IsNullOrEmpty(itm.FirstName) && itm.FirstName.ToLower().Contains(phrase)) 
                || (!string.IsNullOrEmpty(itm.LastName) && itm.LastName.ToLower().Contains(phrase))
                || (!string.IsNullOrEmpty(itm.Email) && itm.Email.ToLower().Contains(phrase))
                );
        }
    }
}
