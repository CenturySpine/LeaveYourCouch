﻿using System.Web;
using System.Web.Mvc;

namespace LeaveYourCouch.Mvc
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CultureFilter(defaultCulture: "fr-FR"));
            filters.Add(new HandleErrorAttribute());
        }
    }
}
