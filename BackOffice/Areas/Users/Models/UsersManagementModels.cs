using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackOffice.Models;
using Core;

namespace BackOffice.Areas.Users.Models
{
    public class UsersManagementIndexViewModel
    {
        public IEnumerable<IBackOfficeUser> Users { get; set; } 
    }


    public class EditUserDialogViewModel : IPersonalAreaDialog
    {
        public IBackOfficeUser User { get; set; }
        public string Caption { get; set; }
        public string Width { get; set; }
    }

    public class EditUserModel : IBackOfficeUser
    {
        public string Create { get; set; }

        public string Id { get; set; }
        public string FullName { get; set; }

        public string IsAdmin { get; set; }
        public string Password { get; set; }
        bool IBackOfficeUser.IsAdmin => !string.IsNullOrEmpty(IsAdmin);
    }
}