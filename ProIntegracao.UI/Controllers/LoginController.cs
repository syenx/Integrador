using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Helper;
using ProIntegracao.UI.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>Login Controller
    /// </summary>
    public class LoginController : BaseController
    {
        #region Variáveis
        private static readonly RepositorioUsuario _repo = new RepositorioUsuario();
        #endregion

        #region Actions

        /// <summary>Index
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// SigIN
        /// </summary>
        /// <param name="username">User Name</param>
        /// <param name="password">PassWord</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SigIn(string username, string password)
        {
            var resultado = false;
            var novousuario = false;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
         //       password = Criptografia.Encrypt(password);

                var user = _repo.BuscarUsuarioLogineSenha(username, password);
                
                if (user != null)
                {
                    CustomPrincipalSerializeModel serializeModel = new CustomPrincipalSerializeModel();
                    serializeModel.Usuario = user;

                    var serializer = new JavaScriptSerializer();

                    string userData = serializer.Serialize(serializeModel);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                             1,
                             username,
                             DateTime.Now,
                             DateTime.Now.AddMinutes(10),
                             false,
                             userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);
                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    Response.Cookies.Add(faCookie);
                    resultado = true;

                    // Gravar Log de Acesso
                    var mensagem = new StringBuilder();

                    mensagem.Append(" Acesso ao Sistema");
                    mensagem.AppendFormat("| Usuário: {0}", username);
                    mensagem.AppendFormat("| Senha: {0}", password);
                    mensagem.AppendFormat("| Data : {0}", DateTime.Now);
                    mensagem.AppendFormat("| IP : {0}", HttpContext.Request.UserHostAddress);

                    if (!string.IsNullOrEmpty(user.Hash))
                        novousuario = true;

               

                }
                else {

                    var mensagem = new StringBuilder();

                    mensagem.Append(" Erro de Acesso ao Sistema");
                    mensagem.AppendFormat("| Usuário: {0}", username);
                    mensagem.AppendFormat("| Senha: {0}", password);
                    mensagem.AppendFormat("| Data : {0}", DateTime.Now);
                    mensagem.AppendFormat("| IP : {0}", HttpContext.Request.UserHostAddress);

                  
                }
            }

            

            return Json(new { Resultado = resultado , NovoUsuario = novousuario }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Sign OUt
        /// </summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Redirect To Local
        /// </summary>
        /// <param name="returnUrl">ReturnUrl</param>
        /// <returns></returns>
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }
        
        /// <summary>Enviar Email Esqueci Minha Senha
        /// 
        /// </summary>
        /// <param name="email">Email de Envio</param>
        /// <param name="usuario">Nome Usuario</param>
        /// <param name="hash">Hash de Verificação</param>
        public void EnviarEmailEsqueciMinhaSenha(string email, string hash)
        {
            var message = new StringBuilder();

            string[] linhas = System.IO.File.ReadAllLines(Server.MapPath("/Content/Template/emailesqueciminhasenha.html"));

            var link = ConfigurationManager.AppSettings["URI"].ToString();

            link = string.Format(@"{0}ResetPassword/{1}", link, hash);

            foreach (var item in linhas)
            {
                message.Append(item.Replace("$link", link));
            }
            string assunto = "Esqueci Minha Senha";

            EnviarMail(email, assunto, message.ToString());
        }
        
        /// <summary>
        /// Esqueci minha Senha
        /// </summary>
        /// <returns></returns>
        public ActionResult EsqueciMinhaSenha()
        {
            ViewBag.Title = "Esqueci Minha Senha";
            return View();
        }

        /// <summary>
        /// Esqueci Minha Senha
        /// </summary>
        /// <param name="Email">Email para Envio</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EsqueciMinhaSenha(string Email)
        {
            //Verifica se o Usuario existe a partir do Email
            var usuario = _repo.Listar().Where(m => m.Email == Email).SingleOrDefault();
            var result = false;
            var mensagem = "";

            if (usuario != null)
            {
                //Cria um novo Hash e atualiza o Registro do usuario
                var novoHash = Guid.NewGuid().ToString();
                usuario.Hash = novoHash;
                result = _repo.Atualizar(usuario);
                if (result)
                {
                    EnviarEmailEsqueciMinhaSenha(Email, novoHash);
                }
            }
            else {
                mensagem = "E-mail não cadastrado";
            }

            return Json(new { Resultado = result, Email = Email, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Alterar Senha
        /// </summary>
        /// <param name="hash">Código de Verificação HASH</param>
        /// <returns>View</returns>
        public ActionResult AlterarSenha(string hash)
        {
            if (string.IsNullOrEmpty(hash)) RedirectToAction("Index");
            var usuario = new Usuario();
            usuario = _repo.BuscarUsuarioLogineSenhaPorHash(hash);
            return View(usuario.Id);
        }

        /// <summary> Alterar Senha
        /// </summary>
        /// <param name="IdUsuario">Id Usuario</param>
        /// <param name="novaSenha">Nova Senha</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AlterarSenha(int IdUsuario, string novaSenha)
        {
            var usuario = new Usuario();
            var resultado = false;
            var message = "";

            usuario = _repo.Obter<Usuario>(IdUsuario);

            if (usuario != null)
            {
                usuario.Senha = Criptografia.Encrypt(novaSenha);
                usuario.Hash = null;
                resultado = _repo.Atualizar(usuario);

                if (resultado)
                {
                    message = string.Format("Senha Alterada para Usuario : {0}", IdUsuario);
                    
                }
            }
            
            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}