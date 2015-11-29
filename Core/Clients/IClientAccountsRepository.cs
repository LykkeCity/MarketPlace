using System.Threading.Tasks;

namespace Core.Clients
{
    public interface IClientAccount
    {
        string Id { get; }
        string Email { get; }
        string Phone { get; }
    }

    public class ClientAccount : IClientAccount
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public interface IClientAccountsRepository
    {
        Task<IClientAccount> RegisterAsync(IClientAccount clientAccount, string password);
        Task<bool> IsTraderWithEmailExistsAsync(string email);
        Task<IClientAccount> AuthenticateAsync(string email, string password);
        Task<IClientAccount> GetByIdAsync(string id);
    }

}
