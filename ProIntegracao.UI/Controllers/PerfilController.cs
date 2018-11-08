using Prointegracao.Data.Entidade;
using ProIntegracao.Data;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;
using ProIntegracao.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Perfil Controller
    /// </summary>
    public class PerfilController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioPerfilPagina _repoPerfilPagina = new RepositorioPerfilPagina();
        private static readonly RepositorioPerfil _repoPerfil = new RepositorioPerfil();
     
        #endregion

        #region Actions

        /// <summary>INDEX
        /// </summary>
        /// <returns>VIEW</returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/Perfil");
            return View();
        }

        /// <summary> Create Perfil - GET
        /// </summary>
        /// <returns>View</returns>
        [Permission(Permissoes.Inserir)]
        public ActionResult Create()
        {
            var viewModel = new PerfilViewModel();
            return View(viewModel);
        }

        /// <summary>Create - Perfil - POST
        /// </summary>
        /// <param name="model">PerfilViewModel</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult Create(PerfilViewModel model)
        {
            var perfil = new Perfil()
            {
                Id              = 0
                , Nome          = model.Nome
                , Admin         = model.Admin
                , DtCadastro    = DateTime.Now
                , DtExclusao    = null
            };
            
            model.ListaPerfilPaginas = model.ParseViewModel(model.ListaPerfilPaginasViewModel);

            var result = false;
            var message = "";

            if (model.ListaPerfilPaginas.Count() > 0)
            {
                try
                {
                    result = _repoPerfil.InserirPerfil(perfil, model.ListaPerfilPaginas);
                  
                }
                catch (Exception ex)
                {
                    var mensagem = string.Format("PERFIL INSERIR CREATE | MENSAGEM : {0}", ex.Message);
                
                }
            }
            else {
                message = "Selecione uma ou mais Permissões.";
            }
            
            return Json(new { Resultado = result , Mensagem = message }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Edit Perfil
        /// </summary>
        /// <param name="idPerfil"></param>
        /// <returns></returns>
        [Permission(Permissoes.Atualizar)]
        public ActionResult Edit(int idPerfil) {

            var perfilPagina = new PerfilViewModel(idPerfil);
            return View(perfilPagina);
        }

        /// <summary>Perfil View Model - Edit
        /// </summary>
        /// <param name="model">PerfilViewModel</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult Edit(PerfilViewModel model)
        {
            var perfil = new Perfil();

            var result = false;

            if (model.Id > 0)
            {
                perfil = _repoPerfil.Listar().Where(m => m.Id == model.Id).FirstOrDefault();
            }
            
            perfil.Id           = model.Id;
            perfil.Nome         = model.Nome;
            perfil.Admin        = model.Admin;
            perfil.DtCadastro   = model.DtCadastro;
            perfil.DtExclusao   = model.DtExclusao;

            model.ListaPerfilPaginas = model.ParseViewModel(model.ListaPerfilPaginasViewModel);
            
            try
            {
                result = _repoPerfil.InserirPerfil(perfil, model.ListaPerfilPaginas);
              
            }
            catch (Exception ex)
            {
                var mensagem = string.Format("PERFIL INSERIR CREATE | MENSAGEM : {0}", ex.Message);
             
            }
            
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Excluir/ Atualizar Perfil Pagina
        /// </summary>
        /// <param name="id">Id do Perfil</param>
        /// <returns>json</returns>
        [Permission(Permissoes.Excluir)]
        public ActionResult Excluir(int id)
        {
            var result = false;

            try
            {
                var perfil = PerfilQuery.ConsultaPerfilPorID(id);
                perfil.DtExclusao = DateTime.Now;
                PerfilQuery.InserirPerfil(perfil);
                result = true;
            
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;

            }

            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Métodos

        /// <summary>Retorna Lista de Páginas
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <param name="ativo">Perfil Ativo</param>
        /// <returns>List PerfilViewModel</returns>
        private IList<PerfilViewModel> RetornaLista(string termo, bool ativo)
        {
            var viewModel = new PerfilViewModel();
            
            //Return MODEL
            return viewModel.ListarPerfilPagina(termo, ativo);
        }

        /// <summary>Listar Perfil
        /// 
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <param name="ativo">Perfil Ativo</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult ListarPerfil(string termo, bool ativo)
        {
            //Recupera Lista de Páginas por parâmetros
            var model = RetornaLista(termo, ativo);

            //retorno
            return PartialView("~/Views/Perfil/_listarPerfil.cshtml", model);
        }

        /// <summary>Listar Pagina Perfil Por Estado
        /// </summary>
        /// <param name="IdEstado">int[] Id Estado</param>
        /// <returns>html</returns>
        [HttpPost]
        public ActionResult ListarPaginaPerfilPorEstado(string[] IdEstado)
        {
            var model = new PerfilViewModel(IdEstado);
            return PartialView("~/Views/Perfil/_listarPerfilPagina.cshtml", model);
        }
        
        /// <summary>Listar Pagina Perfil Por IdPerfil
        /// </summary>
        /// <param name="idPerfil">Id Perfil</param>
        /// <param name="IdEstado">Id do Estado</param>
        /// <returns>html</returns>
        [HttpPost]
        public ActionResult ListarPaginaPerfilPorIdPerfil(int idPerfil, string[] IdEstado)
        {
            var model = new PerfilViewModel(idPerfil, IdEstado);
            return PartialView("~/Views/Perfil/_listarPerfilPagina.cshtml", model);
        }
        
        /// <summary>Listar Pagina Perfil Por Estado Edicao
        /// </summary>
        /// <param name="idPerfil">Id Perfil</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ListarPaginaPerfilPorEstadoEdicao(int idPerfil)
        {
            var model = new PerfilViewModel(idPerfil);
            return PartialView("~/Views/Perfil/_listarPerfilPagina.cshtml", model);
        }
        
        #endregion
    }
}