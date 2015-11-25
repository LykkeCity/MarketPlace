using System.Threading.Tasks;
using System.Web.Mvc;
using BackOffice.Areas.Users.Models;
using BackOffice.Controllers;
using BackOffice.Translates;
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
        public async Task<ActionResult> Index()
        {
            var viewModel = new UsersManagementIndexViewModel
            {
                Users = await _backOfficeUsersRepository.GetAllAsync()
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<ActionResult> EditUserDialog(string id)
        {
            var viewModel = new EditUserDialogViewModel
            {
                User = string.IsNullOrEmpty(id) ? BackOfficeUser.CreateDefault():  await _backOfficeUsersRepository.GetAsync(id),
                Caption = Phrases.EditUser
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<ActionResult> EditUser(EditUserModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#id");

            if (string.IsNullOrEmpty(model.FullName))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#fullName");

            if (string.IsNullOrEmpty(model.Password))
                return this.JsonFailResult(Phrases.FieldShouldNotBeEmpty, "#password");

            if (!string.IsNullOrEmpty(model.Create))
            {
                if (await _backOfficeUsersRepository.UserExists(model.Id))
                    return this.JsonFailResult(Phrases.UserExists, "#id");
            }

            await _backOfficeUsersRepository.SaveAsync(model, model.Password);

            return this.JsonRequestResult("#pamain", Url.Action("Index"));

        }

    }

}