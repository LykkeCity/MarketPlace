using System;
using System.Threading.Tasks;

namespace Core.Clients
{
    public interface IClientAccount
    {
        DateTime Registered { get; }
        string Id { get; }
        string Email { get; }
        string Phone { get; }
    }

    public class ClientAccount : IClientAccount
    {
        public DateTime Registered { get; set; }
        public string Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }

        public static ClientAccount Create(string email, string phone)
        {
            return new ClientAccount
            {
                Email = email,
                Registered = DateTime.UtcNow,
                Phone = phone
            };
        }
    }

    public interface IClientAccountsRepository
    {
        Task<IClientAccount> RegisterAsync(IClientAccount clientAccount, string password);
        Task<bool> IsTraderWithEmailExistsAsync(string email);
        Task<IClientAccount> AuthenticateAsync(string email, string password);
        Task ChangePassword(string clientId, string newPassword);
        Task<IClientAccount> GetByIdAsync(string id);
        Task<IClientAccount> GetByEmailAsync(string email);
    }

}
