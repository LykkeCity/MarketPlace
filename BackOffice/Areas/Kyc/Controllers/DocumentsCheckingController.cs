using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Kyc.Models;
using BackOffice.Controllers;
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
        private readonly IKycDocumentsRepository _kycDocumentsRepository;
        private readonly IKycDocumentsScansRepository _kycDocumentsScansRepository;
        private readonly IPersonalDataRepository _personalDataRepository;
        private readonly IKycRepository _kycRepository;

        public DocumentsCheckingController(SrvKycStatusManager kycStatusManager, SrvClientFinder srvClientFinder, 
            IKycDocumentsRepository kycDocumentsRepository, IKycDocumentsScansRepository kycDocumentsScansRepository, 
            IPersonalDataRepository personalDataRepository, IKycRepository kycRepository)
        {
            _kycStatusManager = kycStatusManager;
            _srvClientFinder = srvClientFinder;
            _kycDocumentsRepository = kycDocumentsRepository;
            _kycDocumentsScansRepository = kycDocumentsScansRepository;
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

            viewModel.Documents = await _kycDocumentsRepository.GetAsync(viewModel.PersonalData.Id);
            viewModel.KycStatus = await _kycRepository.GetKycStatusAsync(viewModel.PersonalData.Id);

            return View(viewModel);
        }

        public async Task<ActionResult> Show(DocumentsShowModel data)
        {
            var doc =
                (await _kycDocumentsRepository.GetAsync(data.ClientId)).FirstOrDefault(
                    itm => itm.DocumentId == data.DocumentId);

            if (doc == null)
                return HttpNotFound();

            var document = await _kycDocumentsScansRepository.GetDocument(doc.DocumentId);

            return File(document, doc.Mime);

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