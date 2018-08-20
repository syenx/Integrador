using ProIntegracao.Data.Entidade;

using ProIntegracao.UI.Controllers;
using ProIntegracao.UI.Models;
using System;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ProIntegracao.UI
{
    /// <summary>
    /// Mvc Application
    /// </summary>
    public class MvcApplication : System.Web.HttpApplication
    {
        /// <summary>
        /// Application_Start
        /// </summary>
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        /// <summary>
        /// Application_PostAuthenticateRequest
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            try
            {
                HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

                if (authCookie != null)
                {
                    FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                    
                    var serializer = new JavaScriptSerializer();

                    CustomPrincipalSerializeModel serializeModel = serializer.Deserialize<CustomPrincipalSerializeModel>(authTicket.UserData);

                    CustomPrincipal novoUsuario = new CustomPrincipal(authTicket.Name);

                    HttpContext.Current.User = novoUsuario;
                }
            }
            catch (CryptographicException)
            {
                FormsAuthentication.SignOut();
            }
            catch (Exception ex)
            {
              
            }
        }

     

        /// <summary> Application Error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;
            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");

            //Caso não seja uma excessão
            if (httpException == null)
            {
                routeData.Values.Add("action", "Index");
            }
            else //Caso seja uma excessão, trato a mesma
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // Página não encontrada.
                        routeData.Values.Add("action", "HttpError404");
                        break;
                    case 500:
                        // Erro interno no servidor.
                        routeData.Values.Add("action", "HttpError500");
                        break;
                    // Erro geral  
                    default:
                        routeData.Values.Add("action", "General");
                        break;
                }
            }

            // Passando detalhes da excessao para o controller.
            routeData.Values.Add("error", exception.Message);

            // Limpando o erro do servidor.
            Server.ClearError();

            // Chamando o controller e passando a rota
            IController errorController = new ErrorController();

            errorController.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        }
    }
}
