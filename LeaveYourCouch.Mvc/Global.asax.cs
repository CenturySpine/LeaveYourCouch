using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using LeaveYourCouch.Mvc.Controllers;
using LeaveYourCouch.Mvc.Migrations;
using LeaveYourCouch.Mvc.Models;
using SimpleInjector;

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

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //string culture = RemoteToolsController._language;
            //if (Request.UserLanguages != null)
            //{
            //    culture = Request.UserLanguages[0];
            //}
            //Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            //Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
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
            using (var txt = new StreamWriter(@"C:\Users\Public\website.logs", true))
            {
                string exformat = ex != null ? "\r\n" + ex.ToString() : string.Empty;
                txt.WriteLine($"{DateTime.Now.ToLongTimeString()} - {source} - {message}{exformat}");
            }
        }
    }



    public class CultureFilter : IAuthorizationFilter
    {
        private readonly string defaultCulture;

        public CultureFilter(string defaultCulture)
        {
            this.defaultCulture = defaultCulture;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string culture = "en-US";
            if (filterContext.HttpContext.Request.UserLanguages != null && filterContext.HttpContext.Request.UserLanguages.Length > 0)
            {
                culture = filterContext.HttpContext.Request.UserLanguages[0];
            }
            //var values = filterContext.RouteData.Values;

            //string culture = (string)values["culture"] ?? this.defaultCulture;

            CultureInfo ci = new CultureInfo(culture);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(ci.Name);
        }
    }
}
