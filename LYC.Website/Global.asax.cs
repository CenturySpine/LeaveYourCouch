using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LYC.Website
{
    //google autocomplete place api key : AIzaSyDtDGjLnw-S73R1l2VcS-5mKZi42R9JXkE
    //google  places api key : AIzaSyC6brf9Bm7PBSXGzDwg2_lD10c-JAMspHo
    //google geocode api key :AIzaSyBb8fFXYm0wuVKU01rgHEVCdBVnA1sMvDc

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
