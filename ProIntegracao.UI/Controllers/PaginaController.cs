using ProIntegracao.Data;
using ProIntegracao.Data.Entidade;
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
    /// Pagina Controller
    /// </summary>
    public class PaginaController : BaseController
    {
        #region Variáveis
        private static readonly RepositorioPagina _repo = new RepositorioPagina();
        #endregion

        #region Ações

        /// <summary>INDEX
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/Pagina");
            var model = new PaginaViewModel();
            return View(model);
        }

        /// <summary> Nova Página - GET
        /// </summary>
        /// <returns>View</returns>
        [Permission(Permissoes.Inserir)]
        public ActionResult Create() {
            var model = new PaginaViewModel();
            return PartialView(model);
        }

        /// <summary>Nova Página - POST
        /// </summary>
        /// <param name="model">Pagina View Model</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult Create(PaginaViewModel model)
        {
            var resultado = false;

            try
            {
                model.DtCadastro = DateTime.Now;
                var pagina = model.ParsePaginaViewModel(model);
                resultado = (_repo.Salvar(pagina) > 0);
             
            }
            catch (Exception ex)
            {
                var message = string.Format("ERRO CREATE PAGINA  | Mensagem : {0}", ex.Message);
        
            }

            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Edit Página
        /// </summary>
        /// <param name="idPerfil">Id da Página</param>
        /// <returns>View</returns>
        [Permission(Permissoes.Atualizar)]
        public ActionResult Edit(int idPerfil)
        {
            var model = new PaginaViewModel(PaginaQuery.ConsultarAcaoPorID(idPerfil));
            return PartialView(model);
        }

        /// <summary>Edit Página - POST
        /// </summary>
        /// <param name="model">pagina View Model</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult Edit(PaginaViewModel model)
        {
            var resultado = false;
            try
            {
                var pagina = model.ParsePaginaViewModel(model);
                resultado = _repo.Atualizar(pagina);
                
            }
            catch (Exception ex)
            {
                var message = string.Format("ERRO EDIT PAGINA  | Mensagem : {0}", ex.Message);
              
            }
            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Excluir/ Atualizar Pagina
        /// </summary>
        /// <param name="id">Id da Página</param>
        /// <returns>json</returns>
        [Permission(Permissoes.Excluir)]
        public ActionResult Excluir(int id)
        {
            var result = false;
            try
            {
                var pagina = _repo.Listar().Where(m => m.Id == id).SingleOrDefault();
                pagina.DtExclusao = DateTime.Now;
                result = (_repo.Atualizar(pagina));
           

            }
            catch (Exception ex)
            {
                var message = string.Format("ERRO DELETE PAGINA  | Mensagem : {0}", ex.Message);
             
            }
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Métodos Privados
        
        /// <summary> Listar Paginas
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <param name="menu">Id Menu</param>
        /// <param name="ativo">Ativo</param>
        /// <returns>PartialView</returns>
        [HttpPost]
        public ActionResult ListarPaginas(string termo, int menu, bool ativo)
        {
            //Recupera Lista de Páginas por parâmetros
            var model = RetornaLista(termo, menu, ativo);

            //retorno
            return PartialView("~/Views/Pagina/_listarPaginas.cshtml", model);
        }
        
        /// <summary>Retorna Lista de Páginas
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <param name="menu">Id Menu</param>
        /// <param name="ativo">Ativo</param>
        /// <returns>IList Pagina </returns>
        private IList<Pagina> RetornaLista(string termo, int menu, bool ativo)
        {
            // variavel
            var model = new List<Pagina>();
            model = _repo.Listar().ToList();
            
            // Verifica o tipo de retorno
            if (!string.IsNullOrEmpty(termo))
                model = model.Where(m => m.Nome.Contains(termo)).ToList();

            if (menu > 0)
                model = model.Where(m => m.Menu.Id == menu).ToList();

            if (ativo)
                model = model.Where(m => m.DtExclusao == null).ToList();
            else
                model = model.Where(m => m.DtExclusao != null).ToList();

            //Return MODEL
            return model;
        }
        
        #endregion
    }
}