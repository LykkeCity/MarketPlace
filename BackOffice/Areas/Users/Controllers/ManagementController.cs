using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Users.Models;
using Core;

namespace BackOffice.Areas.Users.Controllers
{
    [Authorize]
    public class ManagementController : Controller
    {
        private readonly IBackOfficeUsersRepository _backOfficeUsersRepository;

        public ManagementController(IBackOfficeUsersRepository backOfficeUsersRepository)
        {
            _backOfficeUsersRepository = backOfficeUsersRepository;
        }

        [HttpPost]
        public ActionResult Index()
        {
            return View();
        }

    }

}