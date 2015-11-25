using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core;

namespace BackOffice.Models
{
    public class IndexModel
    {
        public string LangId { get; set; }
    }

    public class IndexPageModel
    {
        public IBrowserSession BrowserSession { get; set; }
    }

    public class AuthenticateModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }


    public class MainMenuViewModel
    {
        
    }

}