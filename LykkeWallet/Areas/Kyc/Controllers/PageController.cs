using System.Threading.Tasks;
using System.Web.Mvc;
using Core.Kyc;
using LykkeWallet.Areas.Kyc.Models;
using LykkeWallet.Controllers;

namespace LykkeWallet.Areas.Kyc.Controllers
{
    [Authorize]
    public class PageController : Controller
    {
        private readonly IKycRepository _kycRepository;

        public PageController(IKycRepository kycRepository)
        {
            _kycRepository = kycRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var clientId = this.GetClientId();

            var viewModel = new KycIndexPageViewModel
            {
                KycStatus = await _kycRepository.GetKycStatusAsync(clientId)
            };

            return View(viewModel);
        }
    }
}