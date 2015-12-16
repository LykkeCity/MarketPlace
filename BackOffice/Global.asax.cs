using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BackEnd;

namespace BackOffice
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var dr = Dependencies.CreateDepencencyResolver();
            DependencyResolver.SetResolver(dr);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
