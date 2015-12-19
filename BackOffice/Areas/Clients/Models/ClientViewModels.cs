using BackOffice.Models;
using Core.Clients;

namespace BackOffice.Areas.Clients.Models
{
    public class ClientViewIndexVideModel :IFindClientViewModel
    {
        public string RequestUrl { get; set; }
        public string Div { get; set; }
    }

    public class ClientViewFindViewModel
    {
        public IPersonalData PersonalData { get; set; }

        public IClientSession[] Sessions { get; set; }
    }
}