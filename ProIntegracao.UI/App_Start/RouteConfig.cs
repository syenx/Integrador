using System.Web.Mvc;
using System.Web.Routing;

namespace ProIntegracao.UI
{
    /// <summary>
    /// RouteConfig
    /// </summary>
    public class RouteConfig
    {
        /// <summary>
        /// Register Routes
        /// </summary>
        /// <param name="routes">RouteCollection</param>
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            
            routes.MapRoute(
                name: "EsqueciMinhaSenha",
                url: "ResetPassword/{hash}",
                defaults: new { controller = "Login", action = "AlterarSenha", hash = "" }
            );

            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
           );
        }
    }
}
