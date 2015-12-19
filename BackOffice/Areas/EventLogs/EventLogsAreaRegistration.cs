using System.Web.Mvc;

namespace BackOffice.Areas.EventLogs
{
    public class EventLogsAreaRegistration : AreaRegistration 
    {
        public override string AreaName => "EventLogs";

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "EventLogs_default",
                "EventLogs/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}