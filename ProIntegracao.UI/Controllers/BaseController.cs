using ProIntegracao.Data.Entidade;

using ProIntegracao.UI.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ProIntegracao.Model.Repositorio;
using System.Net.Mail;
using System.Configuration;
using System.Text;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using ProIntegracao.UI.Attribute;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Base Controller
    /// </summary>
    [CheckSessionTimeOut]
    public class BaseController : Controller
    {

        #region Variáveis

        /// <summary>
        /// Repósitorio Página
        /// </summary>
        public static readonly RepositorioPagina _repoPagina = new RepositorioPagina();
        
        #endregion
        
        #region Propriedade

        /// <summary>
        /// Name
        /// </summary>
        public static string Name { get; set; }

        /// <summary> Menu
        /// Lista de Paginas do Menu
        /// </summary>
        public static List<Pagina> Menu { get; set; }

        /// <summary>Usuario Custom Principal
        /// </summary>
        protected virtual new CustomPrincipal User
        {
            get { return HttpContext.User as CustomPrincipal; }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// OnActionExecuting
        /// </summary>
        /// <param name="context"></param>
        protected override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var principal = context.HttpContext.User as CustomPrincipal;
            var controllerName = context.RouteData.Values["Controller"].ToString();

            Name = RetornaNomePagina(controllerName);


            if (principal != null)
            {
                if (!principal.Autenticado)
                {
                    if (controllerName.ToUpper() != "LOGIN")
                        RedirecToLogin(context);
                }

                if (principal != null && principal.Usuario != null)
                {
                    if (!string.IsNullOrEmpty(principal.Usuario.Hash) && controllerName.ToUpper() == "HOME")
                    {
                        RedirecToAlterarSenha(context);
                    }

                    if (principal.Usuario.Perfil.Admin){
                        Menu = _repoPagina.Listar().Where(m=> m.DtExclusao== null).OrderBy(m => m.Ordem).ToList();
                    }
                    else {
                        Menu = principal.ListaPerfilPagina.Select(m => m.Pagina).Where(m => m.DtExclusao == null).DistinctBy(m => m.Nome).ToList();
                    }
                }
            }
            else
            {
                if(controllerName.ToUpper() != "LOGIN")
                    RedirecToLogin(context);
            }
        }

        /// <summary>
        /// Retorna o Nome da Págna Cdastrado
        /// </summary>
        /// <param name="controllerName"></param>
        /// <returns></returns>
        public string RetornaNomePagina(string controllerName)
        {
            var result = string.Empty;

            if (controllerName.ToUpper() == "HOME") return "Home";

            try
            {
                var pagina = _repoPagina.Listar().Where(m => m.Url.ToUpper().Equals(controllerName.ToUpper())).FirstOrDefault();

                if (pagina != null) result = pagina.Nome;
            }
            catch (Exception ex)
            {
              
            }

            return result;
        }

        /// <summary>Redirecionar para Alterar Senha
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        private void RedirecToAlterarSenha(ActionExecutingContext context)
        {
            var url = new UrlHelper(context.RequestContext);
            var loginUrl = url.Content("~/Usuario/AlterarSenha");
            context.HttpContext.Response.Redirect(loginUrl, true);
        }

        /// <summary> Redirecionamento To LoGin
        /// </summary>
        /// <param name="context">ActionExecutingContext</param>
        private static void RedirecToLogin(ActionExecutingContext context)
        {
            var url = new UrlHelper(context.RequestContext);
            var loginUrl = url.Content("~/Login/Index");
            context.HttpContext.Response.Redirect(loginUrl, true);
        }

       
       

        /// <summary>Envio de Mensagem Básico
        /// </summary>
        /// <param name="para">Destinatário</param>
        /// <param name="assunto">Assunto E-mail</param>
        /// <param name="mensagem">Mensagem</param>
        public void EnviarMail(string para, string assunto, string mensagem)
        {
            //Variáveis
            string emailcredential = ConfigurationManager.AppSettings["EmailCredentials"].ToString();
            string passcredential = ConfigurationManager.AppSettings["PasswordCredentials"].ToString();
            assunto = string.Format("{0} | PROINTEGRAÇÃO",assunto);

            //SmtpClient
            var client = new SmtpClient();
            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"].ToString());
            client.Host = ConfigurationManager.AppSettings["SmtpHost"].ToString();
            client.EnableSsl = true;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential(emailcredential, passcredential);

            //ServicePointer
            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            
            //Mail Message
            var mail = new MailMessage(emailcredential,para,assunto,mensagem);
            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                string message = string.Format("Erro Envio Email |Assunto : {0} |Email : {1} | Erro: {2}", assunto, para, ex.Message);
             
            }
        }
        
        #endregion
    }
}