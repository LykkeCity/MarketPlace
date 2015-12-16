using System.Threading.Tasks;
using Core.Clients;

namespace MockServices.Clients
{
    public class SrvSmsConfirmatorMock : ISrvSmsConfirmator
    {
        public Task SendSmsAsync(string clientId)
        {
            return Task.FromResult(0);
        }

        public Task<bool> CheckSmsConfirmation(string clientId, string smsCode)
        {
            return Task.FromResult(smsCode == "1111");
        }
    }
}
