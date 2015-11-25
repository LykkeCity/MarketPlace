using System.Threading.Tasks;

namespace Core
{
    public interface IIdentityGenerator
    {
        Task<int> GenerateNewIdAsync();
    }
}
