using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Clients.Models;
using Core.Clients;
using LkeServices.Clients;

namespace BackOffice.Areas.Clients.Controllers
{
    [Authorize]
    public class ViewController : Controller
    {
        private readonly SrvClientFinder _srvClientFinder;
        private readonly IClientsSessionsRepository _clientsSessionsRepository;

        public ViewController(SrvClientFinder srvClientFinder, IClientsSessionsRepository clientsSessionsRepository)
        {
            _srvClientFinder = srvClientFinder;
            _clientsSessionsRepository = clientsSessionsRepository;
        }

        [HttpPost]
        public ActionResult Index()
        {
            var viewModel = new ClientViewIndexVideModel
            {
                RequestUrl = Url.Action("Find")
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Find(string phrase)
        {
            var viewModel = new ClientViewFindViewModel
            {
                PersonalData = await _srvClientFinder.FindClientAsync(phrase),
                
            };

            if (viewModel.PersonalData == null)
                return View(viewModel);

            viewModel.Sessions =
                (await _clientsSessionsRepository.GetByClientAsync(viewModel.PersonalData.Id)).ToArray();

            return View(viewModel);
        }
    }
}