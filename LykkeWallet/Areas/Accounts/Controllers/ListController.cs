using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using LykkeWallet.Controllers;

namespace LykkeWallet.Areas.Accounts.Controllers
{
    [Authorize]
    public class ListController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Index()
        {
            var kycResult = await this.GetKycStatus();
            if (kycResult != null)
                return kycResult;

            return View();
        }
    }
}