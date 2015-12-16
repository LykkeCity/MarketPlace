﻿using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Common;
using Core.Kyc;
using LkeServices.Kyc;
using LykkeWallet.Areas.Kyc.Models;
using LykkeWallet.Controllers;
using LykkeWallet.Strings;

namespace LykkeWallet.Areas.Kyc.Controllers
{
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IKycDocumentsRepository _kycDocumentsRepository;
        private readonly IKycDocumentsScansRepository _kycDocumentsScansRepository;
        private readonly SrvKycDocumentsManager _srvKycDocumentsManager;
        private readonly SrvKycStatusManager _srvKycStatusManager;

        public DocumentsController(IKycDocumentsRepository kycDocumentsRepository, IKycDocumentsScansRepository kycDocumentsScansRepository, 
            SrvKycDocumentsManager srvKycDocumentsManager, SrvKycStatusManager srvKycStatusManager)
        {
            _kycDocumentsRepository = kycDocumentsRepository;
            _kycDocumentsScansRepository = kycDocumentsScansRepository;
            _srvKycDocumentsManager = srvKycDocumentsManager;
            _srvKycStatusManager = srvKycStatusManager;
        }

        [HttpPost]
        public async Task<ActionResult> UploadFrame(string type)
        {

            var clientId = this.GetClientId();

            var viewModel = new UploadFrameViewModel
            {
                Type = type,
                UploadUrl = Url.Action("Upload"),
                Documents = (await _kycDocumentsRepository.GetAsync(clientId)).Where(itm => itm.Type == type)
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> Upload(string docId)
        {
            if (Request.Files.Count == 0)
                return Json(new { result = "fail" });

            if (Request.Files[0] == null)
                return Json(new { result = "fail" });

            var clientId = this.GetClientId();

            var fileName = Request.Files[0].FileName;
            var mime = Request.Files[0].ContentType;
            var stream = Request.Files[0].InputStream;

            var id = await _srvKycDocumentsManager.UploadDocument(clientId, docId, fileName, mime, stream.ToBytes());

            return Json(new { result = "ok", id });
        }


        public async Task<ActionResult> Get(string id)
        {
            var clientId = this.GetClientId();

            var doc = (await _kycDocumentsRepository.GetAsync(clientId)).FirstOrDefault(itm => itm.DocumentId == id);
            if (doc == null)
                return HttpNotFound();

            var result = await _kycDocumentsScansRepository.GetDocument(id);

            return File(result, doc.Mime);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var clientId = this.GetClientId();
            var doc = await _srvKycDocumentsManager.DeleteAsync(clientId, id);

            return this.JsonShowContentResult("#docsArea" + doc.Type, Url.Action("UploadFrame", new { type = doc.Type}), null, new JsonResultExtParams { ShowLoading = true });
        }

        [HttpPost]
        public async Task<ActionResult> SubmitDocuments()
        {
            var clientId = this.GetClientId();
            var documents = (await _kycDocumentsRepository.GetAsync(clientId)).ToArray();

            if (documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.IdCard) == null)
                return this.JsonFailResult("#panelPickUpFile" + KycDocumentTypes.IdCard, Phrases.PleaseUploadYouPassport);

            if (documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.ProofOfAddress) == null)
                return this.JsonFailResult("#panelPickUpFile" + KycDocumentTypes.ProofOfAddress, Phrases.UploadYouProofOfAddress);

            if (documents.FirstOrDefault(itm => itm.Type == KycDocumentTypes.Selfie) == null)
                return this.JsonFailResult("#panelPickUpFile" + KycDocumentTypes.Selfie, Phrases.MakeSelfiePhoto);

            await _srvKycStatusManager.ChangeKycStatus(clientId, KycStatus.Pending);

            return this.JsonShowContentResult("#pamain", Url.Action("Index","Page", new {area="Kyc"}), null, new JsonResultExtParams { ShowLoading = true });

        }


    }
}