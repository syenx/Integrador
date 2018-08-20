using System;

using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProIntegracao.UI.Attribute
{
    /// <summary>
    /// CheckSessionTimeOutAttribute
    /// </summary>
    public class CheckSessionTimeOutAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(System.Web.Mvc.ActionExecutingContext filterContext)
        {
            var context = filterContext.HttpContext;
            if (context.Session != null)
            {
                if (context.Session.IsNewSession)
                {
                    string sessionCookie = context.Request.Headers["Cookie"];
                    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET&#95;SessionId") >= 0))
                    {
                        FormsAuthentication.SignOut();
                        string redirectTo = "~/Login";

                        if (!string.IsNullOrEmpty(context.Request.RawUrl))
                        {
                            redirectTo = string.Format("~/Login?ReturnUrl={0}", HttpUtility.UrlEncode(context.Request.RawUrl));
                        }

                        filterContext.HttpContext.Response.Redirect(redirectTo, true);
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}