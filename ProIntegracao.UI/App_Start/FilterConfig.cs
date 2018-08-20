﻿using System.Web;
using System.Web.Mvc;

namespace ProIntegracao.UI
{
    /// <summary>Register Filter Config
    /// </summary>
    public class FilterConfig
    {
        /// <summary> Register Global Filters
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
