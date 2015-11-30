using System.Threading.Tasks;
using System.Web.Mvc;
using LykkeWallet.Controllers;

namespace LykkeWallet.Areas.Exchange.Controllers
{
    [Authorize]
    public class AssetsController : Controller
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