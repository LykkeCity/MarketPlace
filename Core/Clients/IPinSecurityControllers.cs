using System.Threading.Tasks;

namespace Core.Clients
{

    public interface IPinSecurityRepository
    {
        Task SaveAsync(string clientId, string pin);
        Task<bool> CheckAsync(string clientId, string pin);
        Task<bool> IsPinEntered(string clientId);
    }

}
