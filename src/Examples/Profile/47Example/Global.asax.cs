using System.Web.Mvc;
using System.Web.Routing;

namespace _47Example
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected static void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}