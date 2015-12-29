using System.Threading.Tasks;
using System.Web.Http;
using Core;
using Wallet_Api.Models;

namespace Wallet_Api.Controllers
{

    public class ClientLogController : ApiController
    {
        private readonly IClientLog _clientLog;

        public ClientLogController(IClientLog clientLog)
        {
            _clientLog = clientLog;
        }

        public Task Post(WriteClientLogModel model)
        {
            if (!string.IsNullOrEmpty(User.Identity?.Name))
            return _clientLog.WriteAsync(User.Identity.Name, model.Data);

            return _clientLog.WriteAsync("ANONIMUS", model.Data);

        }

    }
}
