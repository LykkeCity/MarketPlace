using System.Threading.Tasks;

namespace Core.Clients
{
    public abstract class TraderSettingsBase
    {
        public abstract string GetId();


        public static T CreateDefault<T>() where T : TraderSettingsBase, new()
        {
            if (typeof (T) == typeof (OrdersRequestSettings))
                return OrdersRequestSettings.CreateDefault() as T;

            return new T();
        }
    }


    public class OrdersRequestSettings : TraderSettingsBase
    {

        public override string GetId()
        {
            return "OrderRequest";
        }

        public bool ActiveChecked { get; set; }
        public bool DoneChecked { get; set; }
        public bool CanceledChecked { get; set; }

        public static OrdersRequestSettings CreateDefault()
        {
            return new OrdersRequestSettings
            {
                ActiveChecked = true,
                DoneChecked = true
            };
        }
        
    }



    public interface IClientSettingsRepository
    {
        Task<T> GetSettings<T>(string traderId) where T : TraderSettingsBase, new();
        Task SetSettings<T>(string traderId, T settings) where T : TraderSettingsBase, new();
    }
}
