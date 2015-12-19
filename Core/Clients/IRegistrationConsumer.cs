using System.Threading.Tasks;

namespace Core.Clients
{
    public interface IRegistrationConsumer
    {
        Task ConsumeRegistration(IClientAccount account, string ip, string regId);
    }
}
