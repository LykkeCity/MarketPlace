using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Kyc.Models;
using BackOffice.Controllers;
using BackOffice.Translates;
using Core.Kyc;
using LkeServices.Kyc;

namespace BackOffice.Areas.Kyc.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IKycDocumentsRepository _kycDocumentsRepository;
        private readonly IKycDocumentsScansRepository _kycDocumentsScansRepository;
        private readonly SrvKycDocumentsManager _srvKycDocumentsManager;

        public DocumentsController(IKycDocumentsRepository kycDocumentsRepository, IKycDocumentsScansRepository kycDocumentsScansRepository, 
            SrvKycDocumentsManager srvKycDocumentsManager)
        {
            _kycDocumentsRepository = kycDocumentsRepository;
            _kycDocumentsScansRepository = kycDocumentsScansRepository;
            _srvKycDocumentsManager = srvKycDocumentsManager;
        }

        [HttpPost]
        public async Task<ActionResult> Index(string id)
        {

            var viewModel = new DocumentsListIndexViewModel
            {
                Documents = await _kycDocumentsRepository.GetAsync(id)
            };
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
        public ActionResult DeletDialog(DeleteDocumentModel model)
        {
            var viewModel = new DeleteDialogViewModel
            {
                Model = model,
                Caption = Phrases.PleaseConfirmYourAction
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteDocument(DeleteDocumentModel model)
        {
            await _srvKycDocumentsManager.DeleteAsync(model.ClientId, model.DocumentId);
            return this.JsonRequestResult("#clientDocuments", Url.Action("Index", new {id = model.ClientId}));
        }

    }
}