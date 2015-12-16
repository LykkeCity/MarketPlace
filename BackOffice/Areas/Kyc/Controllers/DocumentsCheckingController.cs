using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Kyc.Models;
using BackOffice.Controllers;
using BackOffice.Translates;
using Core.Clients;
using Core.Kyc;
using LkeServices.Clients;
using LkeServices.Kyc;

namespace BackOffice.Areas.Kyc.Controllers
{
    [Authorize]
    public class DocumentsCheckingController : Controller
    {
        private readonly SrvKycStatusManager _kycStatusManager;
        private readonly SrvClientFinder _srvClientFinder;
        private readonly IPersonalDataRepository _personalDataRepository;
        private readonly IKycRepository _kycRepository;

        public DocumentsCheckingController(SrvKycStatusManager kycStatusManager, SrvClientFinder srvClientFinder, 
            IPersonalDataRepository personalDataRepository, IKycRepository kycRepository)
        {
            _kycStatusManager = kycStatusManager;
            _srvClientFinder = srvClientFinder;
            _personalDataRepository = personalDataRepository;
            _kycRepository = kycRepository;
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var viewModel = new DocumentsCheckingIndexViewModel
            {
                ClientAccounts = await _kycStatusManager.GetAccountsToCheck(),
                RequestUrl = Url.Action("FindClient")
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> FindClient(string phrase)
        {
            var viewModel = new DocumentsCheckingFindClientViewModel
            {
                PersonalData = await _srvClientFinder.FindClientAsync(phrase)
            };

            if (viewModel.PersonalData == null)
                return View(viewModel);

            viewModel.KycStatus = await _kycRepository.GetKycStatusAsync(viewModel.PersonalData.Id);

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Change(UpdateDocumentsAndStatusModel data)
        {
            await _personalDataRepository.UpdateAsync(data);
            await _kycStatusManager.ChangeKycStatus(data.Id, data.KycStatus);
            return this.JsonRequestResult("#pamain", Url.Action("Index"));
        }


    }
}