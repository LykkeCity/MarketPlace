using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LykkeWallet.Areas.PersonalData.Controllers
{
    [Authorize]
    public class PageController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }
    }
}