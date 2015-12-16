using System.Collections.Generic;
using Core.Assets;
using Core.Clients;
using Core.Orders;

namespace LykkeMarketPlace.Models
{

    public class OrdersIndexViewModel
    {
        public OrdersRequestSettings OrdersRequestSettings { get; set; }
    }


    public class GetOrdersModel
    {
        public string Active { get; set; }
        public string Done { get; set; }
        public string Canceled { get; set; }
    }

    public class GetOrdersViewModel
    {

        public Dictionary<string, IAssetPair> AssetPairs { get; set; } 

        public OrderBase[] Orders { get; set; }

    }
}