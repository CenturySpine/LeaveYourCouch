using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LeaveYourCouch.Services;


namespace LeaveYourCouch.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            //// Create the container as usual.
            //var container = new Container();
            //container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();


            //// This is an extension method from the integration package.
            //container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

            //container.Verify();

            //GlobalConfiguration.Configuration.DependencyResolver =
            //    new SimpleInjectorWebApiDependencyResolver(container);


            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
