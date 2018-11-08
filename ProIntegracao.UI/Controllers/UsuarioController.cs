using ProIntegracao.Data;
using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;
using ProIntegracao.UI.Helper;
using ProIntegracao.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>UsuarioController
    /// </summary>
    public class UsuarioController : BaseController
    {
      
        private static readonly RepositorioUsuario _repo = new RepositorioUsuario();

        /// <summary>
        /// 
        /// </summary>
        [Permission(Permissoes.Admin)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/Usuario");
            return View();
        }

        /// <summary> Novo Usuario
        /// Utilizando ViewModel como Container
        /// </summary>
        /// <returns>View</returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Create()
        {
            var model = new UsuarioViewModel();
            return PartialView(model);
        }

        /// <summary> Novo Usuario - POST
        /// </summary>
        /// <param name="model">Usuario View Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(UsuarioViewModel model)
        {
            var resultado = false;

            try
            {
                var senha = string.Format("{0}{1}{2}", DateTime.Now.Hour.ToString()
                                                     , DateTime.Now.Minute.ToString()
                                                     , DateTime.Now.Second.ToString()
                                                     );

                //Ajustes
                var usuario = model.ParseUsuarioViewModel(model);

                // Criar Usuario
                usuario.Senha = senha;// Criptografia.Encrypt(senha);
                resultado = (_repo.Salvar(usuario) > 0);

                //E-mail
                EnviarEmailNovoUsuario(usuario.Email, usuario.Login, senha);

                //LOG
             
            }
            catch (Exception ex)
            {
                //Log de Erro
                var msgErro = ex.Message;
            }

            //Retorno
            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary> Editar Usuario
        /// </summary>
        /// <param name="id">Id do Usuario</param>
        /// <returns>html</returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Edit(int id)
        {
            var model = new UsuarioViewModel(UsuarioQuery.ConsultarUsuarioPorID(id));
            return PartialView(model);
        }

        /// <summary>Editar Usuario - POST
        /// </summary>
        /// <param name="model">Usuario View MODEL</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult Edit(UsuarioViewModel model)
        {
            var resultado = false;

            try
            {
                var usuario = model.ParseUsuarioViewModel(model);
                resultado = _repo.Atualizar(usuario);
                //InserirLog(controllername, "EDIT", "INFO");
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog(controllername, ex.Message);
            }

            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary> Excluir Usuario
        /// </summary>
        /// <param name="id">Id do Usuario</param>
        /// <returns></returns>
        [HttpPost]
        [Permission(Permissoes.Admin)]
        public ActionResult Excluir(int id)
        {

            var result = false;
            try
            {
                var usuario = UsuarioQuery.ConsultarUsuarioPorID(id);
                usuario.DtExclusao = DateTime.Now;
                UsuarioQuery.Atualizar(usuario);
                //InserirLog(controllername, "EXCLUIR", "INFO");
                result = true;
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog(controllername, ex.Message);
            }
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Alterar Senha
        /// </summary>
        /// <returns></returns>
        public ActionResult AlterarSenha()
        {
            return View();
        }

        /// <summary>Alterar Senha
        /// </summary>
        /// <param name="senhaAtual">Senha Atual</param>
        /// <param name="novaSenha">Nova Senha</param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult AlterarSenha(string senhaAtual, string novaSenha)
        {
            var resultado = false;
            var mensagem = string.Empty;
            var usuario = _repo.Obter<Usuario>(User.Usuario.Id);

            if (/*Criptografia.Encrypt(senhaAtual)*/senhaAtual == usuario.Senha)
            {
                usuario.Senha = novaSenha;//Criptografia.Encrypt(novaSenha);
                usuario.Hash = null;
                resultado = _repo.Atualizar(usuario);

                if (resultado) EnviarEmailAlteracaoSenha(usuario.Email);
            }
            else {
                mensagem = "Senha Atual não confere";
            }
            return Json(new { Resultado = resultado, Mensagem = mensagem }, JsonRequestBehavior.AllowGet);
        }



        #region Métodos

        /// <summary> Verificar se o Usuário já esta cadastrado
        /// </summary>
        /// <param name="Login">LOGIN do USUÁRIO</param>
        /// <returns>string</returns>
        public ActionResult VerificaUsuario(string Login)
        {
            var usuario = new Usuario();

            var result = "true"; //necessário pois é requisito no stringfy do jquery.validate

            try
            {
                usuario = _repo.Listar().Where(m => m.Login.ToUpper() == Login.ToUpper()).FirstOrDefault();
                if (usuario != null) result = "*LOGIN já existe";
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;

            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Listar Todos os Usuarios por Termo
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <param name="ativo">Ativo</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult ListarUsuarios(string termo, bool ativo)
        {
            //Recupera Lista de Usuários por parâmetros
            var model = RetornaLista(termo, ativo);

            //retorno
            return PartialView("~/Views/Usuario/_listarUsuarios.cshtml", model);
        }

        /// <summary>
        /// Retorna Lista
        /// </summary>
        /// <param name="termo">Termo</param>
        /// <param name="ativo">Ativo</param>
        /// <returns></returns>
        public IList<Usuario> RetornaLista(string termo, bool ativo)
        {
            // VARIAVEL
            var model = new List<Usuario>();

            model = _repo.Listar().ToList();

            // Verifica o tipo de retorno
            if (!string.IsNullOrEmpty(termo))
                model = model.Where(m => m.Login.ToLower().Contains(termo.ToLower()) || m.Email.ToLower().Contains(termo.ToLower())).ToList();
            
            // ATIVO
            if (ativo)
                model = model.Where(m => m.DtExclusao == null).ToList();
            else
                model = model.Where(m => m.DtExclusao != null).ToList();
            
            return model.OrderBy(x => x.Login).ToList(); ;
        }
        
        /// <summary>Enviar Email Novo Usuario
        /// </summary>
        /// <param name="email">E-mail</param>
        /// <param name="usuario">Usuario</param>
        /// <param name="password">Senha</param>
        public void EnviarEmailNovoUsuario(string email, string usuario, string password)
        {
            var message = new StringBuilder();

            string[] linhas = System.IO.File.ReadAllLines(Server.MapPath("/Content/Template/emailnovousuario.html"));

            var link = ConfigurationManager.AppSettings["URI"].ToString();

            foreach (var item in linhas)
            {
                message.Append(item.Replace("$link", link).Replace("$usuario", usuario).Replace("$senha", password));
            }
            string assunto = "Novo Usuario";
            EnviarMail(email, assunto, message.ToString());
        }

        /// <summary>Enviar Email Alteração Senha
        /// </summary>
        /// <param name="email">E-mail</param>
        public void EnviarEmailAlteracaoSenha(string email)
        {
            var message = new StringBuilder();

            string[] linhas = System.IO.File.ReadAllLines(Server.MapPath("/Content/Template/emailalteracaosenha.html"));

            var link = ConfigurationManager.AppSettings["URI"].ToString();

            foreach (var item in linhas)
            {
                message.Append(item);
            }
            string assunto = "Alterãção Senha";

            EnviarMail(email, assunto, message.ToString());
        }

        #endregion
    }
}
