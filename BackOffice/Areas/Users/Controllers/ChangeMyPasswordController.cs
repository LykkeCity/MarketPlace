using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Users.Models;
using BackOffice.Controllers;
using BackOffice.Translates;
using Core;

namespace BackOffice.Areas.Users.Controllers
{
    [Authorize]
    public class ChangeMyPasswordController : Controller
    {
        private readonly IBackOfficeUsersRepository _backOfficeUsersRepository;

        public ChangeMyPasswordController(IBackOfficeUsersRepository backOfficeUsersRepository)
        {
            _backOfficeUsersRepository = backOfficeUsersRepository;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> ChangeMyPassword(ChangeMyPasswordModel model)
        {

            if (string.IsNullOrEmpty(model.NewPassword))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#pswd");

            if (string.IsNullOrEmpty(model.PasswordConfirmation))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#pswdConf");

            if (model.NewPassword != model.PasswordConfirmation)
                return this.JsonFailResult(Phrases.PasswordsDoNotMatch, "#pswdConf");

            var id = this.GetUserId();
            await _backOfficeUsersRepository.ChangePasswordAsync(id, model.NewPassword);
            return this.JsonFailResult(Phrases.PasswordIsNowChanged, "#btnChangePassword");
        }
    }
}