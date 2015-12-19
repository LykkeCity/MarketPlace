using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.EventLogs.Models;
using BackOffice.Controllers;
using BackOffice.Translates;
using Core.EventLogs;

namespace BackOffice.Areas.EventLogs.Controllers
{
    [Authorize]
    public class RegistrationsController : Controller
    {
        private readonly IRegistrationLogs _registrationLogs;

        public RegistrationsController(IRegistrationLogs registrationLogs)
        {
            _registrationLogs = registrationLogs;
        }


        [HttpPost]
        public ActionResult Index()
        {
            var viewModel = new EvntLogsRegsIndexViewModel
            {
                To = DateTime.UtcNow.Date
            };

            viewModel.From = viewModel.To.AddDays(-1);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> GetData(GetEvntLogsRegsModel model)
        {
            if (model.From == null)
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#from");

            if (model.To == null)
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#to");


            var viewModel = new GetEvntLogsRegsViewModel
            {
                Events = (await _registrationLogs.GetAsync(model.From.Value, model.To.Value)).ToArray()
            };


            return View(viewModel);
            
        }
    }
}