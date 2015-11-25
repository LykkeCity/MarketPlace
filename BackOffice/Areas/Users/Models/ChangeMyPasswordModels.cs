using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackOffice.Areas.Users.Models
{
    public class ChangeMyPasswordModel
    {
        public string NewPassword { get; set; }
        public string PasswordConfirmation { get; set; }
    }

}