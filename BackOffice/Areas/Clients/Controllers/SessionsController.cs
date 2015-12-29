using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Clients.Models;
using BackOffice.Controllers;
using BackOffice.Translates;
using Core.Clients;

namespace BackOffice.Areas.Clients.Controllers
{
    [Authorize]
    public class SessionsController : Controller
    {
        private readonly IClientsSessionsRepository _clientsSessionsRepository;

        public SessionsController(IClientsSessionsRepository clientsSessionsRepository)
        {
            _clientsSessionsRepository = clientsSessionsRepository;
        }

        [HttpPost]
        public ActionResult DeleteConfirmationDialog(DeleteSessionModel model)
        {
            var viewModel = new DeleteSessionConfirmationDialogViewModel
            {
                Caption = Phrases.PleaseConfirmYourAction,
                Data = model
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteSession(DeleteSessionModel model)
        {
            await _clientsSessionsRepository.DeleteSessionAsync(model.ClientId, model.Token);
            return this.JsonResultShowDialog(Url.Action("DeletedDialog"));
        }

        [HttpPost]
        public ActionResult DeletedDialog()
        {
            var viewModel = new SessionDeletedDialogViewModel
            {
                Caption = Phrases.SessionHasBeenDeleted
            };


            return View(viewModel);
        }
    }
}