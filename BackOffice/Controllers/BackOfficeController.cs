using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Core.BackOffice;

namespace BackOffice.Controllers
{
    [Authorize]
    public class BackOfficeController : Controller
    {
        private readonly IMenuBadgesRepository _menuBadgesRepository;

        public BackOfficeController(IMenuBadgesRepository menuBadgesRepository)
        {
            _menuBadgesRepository = menuBadgesRepository;
        }

        [HttpPost]
        public ActionResult Layout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Menu()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> GetBadges()
        {
            var badges = await _menuBadgesRepository.GetBadesAsync();
            return Json(badges.Select(itm => new {id = itm.Id, value = itm.Value}));
        }

    }
}