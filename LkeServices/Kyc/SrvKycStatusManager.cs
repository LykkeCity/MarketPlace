using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.BackOffice;
using Core.Clients;
using Core.Kyc;

namespace LkeServices.Kyc
{
    public class SrvKycStatusManager
    {
        private readonly IKycRepository _kycRepository;
        private readonly IMenuBadgesRepository _menuBadgesRepository;
        private readonly IPersonalDataRepository _personalDataRepository;

        public SrvKycStatusManager(IKycRepository kycRepository, IMenuBadgesRepository menuBadgesRepository, 
            IPersonalDataRepository personalDataRepository)
        {
            _kycRepository = kycRepository;
            _menuBadgesRepository = menuBadgesRepository;
            _personalDataRepository = personalDataRepository;
        }

        private async Task UpdateKycBadge()
        {
            var count = (await _kycRepository.GetClientsByStatus(KycStatus.Pending)).Count();
            await _menuBadgesRepository.SaveBadgeAsync(MenuBadges.Kyc, count.ToString());
        }

        public async Task ChangeKycStatus(string clientId, KycStatus kycStatus)
        {
            await _kycRepository.SetStatusAsync(clientId, kycStatus);
            await UpdateKycBadge();
        }


        public async Task<IEnumerable<IPersonalData>> GetAccountsToCheck()
        {
            var ids = await _kycRepository.GetClientsByStatus(KycStatus.Pending);
            return await _personalDataRepository.GetAsync(ids);
        }

    }

}
