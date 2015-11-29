using System.Web.Mvc;
using LykkeWallet.Services;

namespace LykkeWallet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string langId)
        {
            if (!string.IsNullOrEmpty(langId))
            {
                this.SetLanguage(langId);
                return Redirect(Url.Action("Index"));
            }

            return View();
        }
    }
}