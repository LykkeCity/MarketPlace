using System.Web.Mvc;

namespace LykkeWallet.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GetMenu()
        {
            return View();
        }

    }
}