using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LykkeWallet.Areas.Settings.Controllers
{
    [Authorize]
    public class LanguageController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }
    }
}