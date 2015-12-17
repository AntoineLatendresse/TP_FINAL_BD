using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DEMO_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start()
        {
            String DB_MainDB_Path = Server.MapPath(@"~\App_Data\MainBD.mdf");
            Session["Main_BD"] = @"Data Source=(LocalDB)\v11.0;AttachDbFilename='" + DB_MainDB_Path + "'; Integrated Security=true; Max Pool Size=1024; Pooling=true;";
        }
    }
}
