using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LeaveYourCouch.Mvc.Migrations;
using LeaveYourCouch.Mvc.Models;

namespace LeaveYourCouch.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            SimpleLogger.Log("MvcApplication.Application_Start", "Starting app");

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            DbcontextTools.Create();

        }
    }

    class DbcontextTools
    {
        public static void Create()
        {
            try
            {
                Database.SetInitializer<ApplicationDbContext>(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
                SimpleLogger.Log("DbcontextTools.Create", "No major events");

            }
            catch (Exception ex)
            {
                SimpleLogger.Log("DbcontextTools.Create", "Error while creating DB", ex);

            }
        }
    }

    public class SimpleLogger
    {
        public static void Log(string source, string message, Exception ex = null)
        {
            using (var txt = new StreamWriter(@"C:\Users\Public\website.logs",true))
            {
                string exformat = ex != null ? "\r\n" + ex.ToString() : string.Empty;
                txt.WriteLine($"{DateTime.Now.ToLongTimeString()} - {source} - {message}{exformat}");
            }
        }
    }
}
