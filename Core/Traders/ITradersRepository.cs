
using System.Threading.Tasks;

namespace Core.Traders
{
    public interface ITrader
    {
        string Id { get; }
        string Email { get; }
    }


    public class Trader : ITrader
    {
        public string Id { get; set; }
        public string Email { get; set; }
    }

    public interface ITradersRepository
    {

        Task<ITrader> RegisterAsync(ITrader trader, string password);

        Task<bool> IsTraderWithEmailExistsAsync(string email);

        Task<ITrader> AuthenticateAsync(string email, string password);
        Task<ITrader> GetByIdAsync(string id);
    }
}
