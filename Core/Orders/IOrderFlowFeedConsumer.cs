using System.Threading.Tasks;

namespace Core.Orders
{
    public enum BookOrderType
    {
        Bid,
        Ask
    }
    public class LimitOrderBookItem
    {
        public double Volume { get; set; }
        public double Rate { get; set; }
        public BookOrderType Type { get; set; }
    }

    public class LimitOrderBookModel
    {
        public string Asset { get; set; }
        public LimitOrderBookItem[] Items { get; set; }

        public static LimitOrderBookModel CreateEmpty(string asset)
        {

            return new LimitOrderBookModel
            {
                Asset = asset,
                Items = new LimitOrderBookItem[0]
            };

        }
        
    }

    public interface IOrderBookChangesConsumer
    {
        Task OrderBookChanged(string asset);
    }
}
