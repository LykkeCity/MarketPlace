using System.Web.Mvc;

namespace LykkeWallet.Areas.Settings.Controllers
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