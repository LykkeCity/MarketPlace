
using System;
using System.Collections.Generic;
using BackOffice.Models;
using Core.Clients;
using Core.Kyc;

namespace BackOffice.Areas.Kyc.Models
{
    public class DocumentsCheckingIndexViewModel : IFindClientViewModel
    {
        public IEnumerable<IPersonalData> ClientAccounts { get; set; }
        public string RequestUrl { get; set; }
        public string Div { get; set; }
    }

    public class DocumentsCheckingFindClientViewModel
    {
        public IPersonalData PersonalData { get; set; }

        public KycStatus KycStatus {get;set;}
    }

    public class DocumentsShowModel
    {
        public string ClientId { get; set; }
        public string DocumentId { get; set; }
    }


    public class UpdateDocumentsAndStatusModel : IPersonalData
    {
        public DateTime Regitered { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public KycStatus KycStatus { get; set; }
    }


    public class DeleteDialogViewModel : IPersonalAreaDialog
    {
        public string Caption { get; set; }
        public string Width { get; set; }
        public DeleteDocumentModel Model { get; set; }
    }


    public class DeleteDocumentModel
    {
        public string ClientId { get; set; }
        public string DocumentId { get; set; }
    }

}