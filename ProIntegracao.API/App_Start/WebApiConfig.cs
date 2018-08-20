using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace ProIntegracao.API
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ExecutaGET",
                routeTemplate: "api/{controller}/{numeroProcesso}/{versao}",
                defaults: new { numeroProcesso = RouteParameter.Optional, versao = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
              name: "ExecutaPOST",
              routeTemplate: "api/{controller}/{ocorrencia}"

          );
        }
    }
}
