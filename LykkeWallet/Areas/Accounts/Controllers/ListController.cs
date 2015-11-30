using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LykkeWallet.Areas.Accounts.Controllers
{
    [Authorize]
    public class ListController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }
    }
}