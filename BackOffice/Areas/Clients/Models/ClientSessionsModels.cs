using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackOffice.Models;

namespace BackOffice.Areas.Clients.Models
{
    public class DeleteSessionModel
    {
        public string ClientId { get; set; }
        public string Token { get; set; }

    }

    public class DeleteSessionConfirmationDialogViewModel : IPersonalAreaDialog
    {
        public string Caption { get; set; }
        public string Width { get; set; }
        public DeleteSessionModel Data { get; set; }
    }

    public class SessionDeletedDialogViewModel : IPersonalAreaDialog
    {
        public string Caption { get; set; }
        public string Width { get; set; }
    }
}