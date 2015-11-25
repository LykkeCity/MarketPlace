using System.Threading.Tasks;

namespace Core.Orders
{
    public interface IOrderExecuter
    {
        Task Execute();
    }
}
