using System.Threading.Tasks;

namespace Core
{
    public interface IRequestsLog
    {
        Task WriteAsync(string userId, string url, string request, string response);
    }
}
