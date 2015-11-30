using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LykkeWallet.Areas.Exchange.Controllers
{
    [Authorize]
    public class AssetsController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }
    }
}