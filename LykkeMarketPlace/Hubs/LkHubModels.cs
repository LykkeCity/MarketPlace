
namespace LykkeMarketPlace.Hubs
{
    public class LkeHubDealModel
    {
        public string CurFrom { get; set; }
        public string CurTo { get; set; }
        public string Volume { get; set; }
        public string Action { get; set; }
    }

    public class LkeHubLimitOrderModel
    {
        public string CurFrom { get; set; }
        public string CurTo { get; set; }
        public string Volume { get; set; }
        public string Action { get; set; }
        public string Price { get; set; }
    }

    public class LkeHubLimitOrder2Model
    {
        public string CurFrom { get; set; }
        public string CurTo { get; set; }
        public string Volume { get; set; }
        public string Action { get; set; }
        public string Bid { get; set; }
        public string Ask { get; set; }

    }


    public class LkeHubOrderBookItem
    {
        public double v { get; set; }
        public double r { get; set; }

    }


    public class LkeMessageModel
    {
        public string DivId { get; set; }
        public string Message { get; set; }

        public static LkeMessageModel Create(string divId, string msg)
        {
            return new LkeMessageModel
            {
                DivId = divId,
                Message = msg
            };
        }
    }
}