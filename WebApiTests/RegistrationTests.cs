using System;
using System.Linq;
using Core.Kyc;
using LkeServices.Kyc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Wallet_Api.Controllers;
using Wallet_Api.Models;
using WebApiTests.Mocks;

namespace WebApiTests
{
    [TestClass]
    public class RegistrationTests
    {
        /// <summary>
        /// Testing one usual usecase of client - Registration and KYC;
        /// 1. Registering;
        /// 2. Uploading documents;
        /// 3. Changing KYC client status to - Pending (checking by KYC complience officer);
        /// 4. Delete Proof of addres (lets say complience officer believe it's a rubbish  document);
        /// 5. Put client back to the KYC- need to fill data status;
        /// 6. Client understands that we have to upload just one document;
        /// 7. Upload deleted document;
        /// 8. Change Kyc status to pending;
        /// 7. After KYC officer approves docs;
        /// 8. Making client registered;
        /// </summary>
        [TestMethod]
        public void TestRegistration()
        {
            var ioc = EnvironmentCreator.CreateEnvironment();

            var client = ioc.RegisterClient("test@test.tt");
            client.SetIdentity();

            var docsToUploadController = ioc.CreateInstance<CheckDocumentsToUploadController>();

            var docsToUpload = docsToUploadController.Get().Result;

            Assert.IsTrue(docsToUpload.Result.IdCard);
            Assert.IsTrue(docsToUpload.Result.ProofOfAddress);
            Assert.IsTrue(docsToUpload.Result.Selfie);

            var kycDocuments = ioc.CreateInstance<KycDocumentsController>();

            var response = kycDocuments.Post(new KycDocumentsModel {Data = Convert.ToBase64String(new byte[]{0,1,2,3,4,5}), Ext = "JPG", Type = KycDocumentTypes.IdCard }).Result;
            Assert.IsNull(response.Error);

            response = kycDocuments.Post(new KycDocumentsModel { Data = Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4, 5 }), Ext = "JPG", Type = KycDocumentTypes.Selfie }).Result;
            Assert.IsNull(response.Error);

            response = kycDocuments.Post(new KycDocumentsModel { Data = Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4, 5 }), Ext = "JPG", Type = KycDocumentTypes.ProofOfAddress }).Result;
            Assert.IsNull(response.Error);

            // Changing Kyc Status to pending
            var changeKycStatusResult = ioc.CreateInstance<KycStatusController>().Post().Result;
            Assert.IsNull(changeKycStatusResult.Error);

            var kycStatusFromControlelr = ioc.CreateInstance<KycStatusController>().Get().Result;
            Assert.AreEqual(kycStatusFromControlelr.Result.KycStatus, KycStatus.Pending.ToString());

            var documens = ioc.GetObject<IKycDocumentsRepository>().GetAsync(client.Id).Result.ToArray();

            var proofOfAddressDoc = documens.First(itm => itm.Type == KycDocumentTypes.ProofOfAddress);

            // Delete wrong document
            ioc.GetObject<SrvKycDocumentsManager>().DeleteAsync(client.Id, proofOfAddressDoc.DocumentId).Wait();

            ioc.GetObject<IKycRepository>().SetStatusAsync(client.Id, KycStatus.NeedToFillData).Wait();

            kycStatusFromControlelr = ioc.CreateInstance<KycStatusController>().Get().Result;
            Assert.AreEqual(kycStatusFromControlelr.Result.KycStatus, KycStatus.NeedToFillData.ToString());

            docsToUpload = docsToUploadController.Get().Result;

            Assert.IsFalse(docsToUpload.Result.IdCard);
            Assert.IsTrue(docsToUpload.Result.ProofOfAddress);
            Assert.IsFalse(docsToUpload.Result.Selfie);

            response = kycDocuments.Post(new KycDocumentsModel { Data = Convert.ToBase64String(new byte[] { 0, 1, 2, 3, 4, 6 }), Ext = "JPG", Type = KycDocumentTypes.ProofOfAddress }).Result;
            Assert.IsNull(response.Error);

            // Changing Kyc Status to pending
            changeKycStatusResult = ioc.CreateInstance<KycStatusController>().Post().Result;
            Assert.IsNull(changeKycStatusResult.Error);

            kycStatusFromControlelr = ioc.CreateInstance<KycStatusController>().Get().Result;
            Assert.AreEqual(kycStatusFromControlelr.Result.KycStatus, KycStatus.Pending.ToString());


            ioc.GetObject<IKycRepository>().SetStatusAsync(client.Id, KycStatus.Ok).Wait();
            kycStatusFromControlelr = ioc.CreateInstance<KycStatusController>().Get().Result;
            Assert.AreEqual(kycStatusFromControlelr.Result.KycStatus, KycStatus.Ok.ToString());
        }

    }
}
