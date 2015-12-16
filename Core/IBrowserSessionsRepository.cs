using System.Threading.Tasks;

namespace Core
{
    public interface IBrowserSession
    {
        string Id { get; }
        string UserName { get; }
    }


    public class BrowserSession : IBrowserSession
    {
        public string Id { get; set; }
        public string UserName { get; set; }
    }

    public interface IBrowserSessionsRepository
    {
        Task<IBrowserSession> GetSessionAsync(string sessionId);
        Task SaveSessionAsync(string sessionId, string userId);
    }
}
