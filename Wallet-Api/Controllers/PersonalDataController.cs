using System.Threading.Tasks;
using System.Web.Http;
using Core.Clients;
using Wallet_Api.Models;

namespace Wallet_Api.Controllers
{
    [Authorize]
    public class PersonalDataController : ApiController
    {
        private readonly IPersonalDataRepository _personalDataRepository;

        public PersonalDataController(IPersonalDataRepository personalDataRepository)
        {
            _personalDataRepository = personalDataRepository;
        }

        public async Task<ResponseModel<ApiPersonalDataModel>> Get()
        {
            var clientId = this.GetClientId();
            var personalData = await _personalDataRepository.GetAsync(clientId);
            return ResponseModel<ApiPersonalDataModel>.CreateOk(personalData.ConvertToApiModel());
        }
    }
}
