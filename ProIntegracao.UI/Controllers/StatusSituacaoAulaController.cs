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
    /// Status Situacação Aula Controller
    /// </summary>
    public class StatusSituacaoAulaController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioStatusSituacaoAula _repo = new RepositorioStatusSituacaoAula();

        #endregion

        #region Actions

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/StatusSituacaoAula");
            var model = new StatusSituacaoAulaViewModel();
            return View(model);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Create()
        {
            var model = new StatusSituacaoAulaViewModel();
            return PartialView(model);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(StatusSituacaoAulaViewModel model)
        {
            var resultado = false;
            var message = "";

            //Verifica se ja existe um registro com os mesmos dados
            if (!VerificaRegistroExistente(model))
            {
               
                try
                {
                    model.DtCadastro = DateTime.Now;
                    var status = model.ParseStatusViewModel(model);
                    resultado = (_repo.Salvar(status) > 0);
                    
                }
                catch (Exception ex)
                {
                 

                }
            }
            else
            {
                message = "Já existe um registro com esses critérios, verifique os dados e tente novamente.";
            }
           
            return Json(new { Resultado = resultado, Mensagem = message }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Verifica Registro Existente Antes de salver o registro
        /// </summary>
        /// <param name="model">StatusSituacaoAulaViewModel object</param>
        /// <returns></returns>
        public bool VerificaRegistroExistente(StatusSituacaoAulaViewModel model)
        {
            var status = _repo.Listar()
                    .Where(m => m.Estado.Id == model.IdEstado 
                    && m.Identificador == model.Identificador 
                    && m.Nome.ToUpper() == model.Nome.ToUpper()
                    ).FirstOrDefault();

            return (status != null);
        }
        
        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="idStatus"></param>
        /// <returns></returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Edit(int idStatus)
        {
            var status = _repo.Listar().Where(m => m.Id == idStatus).FirstOrDefault();
            var model = new StatusSituacaoAulaViewModel(status);
            return PartialView(model);
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="model">StatusSituacaoAulaViewModel</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(StatusSituacaoAulaViewModel model)
        {
            var resultado = false;
            try
            {
                var status = model.ParseStatusViewModel(model);
                resultado = _repo.Atualizar(status);
                //InserirLog("STATUS SITUACAO", "EDIT");
            }
            catch (Exception ex)
            {
                //InserirLog("STATUS SITUACAO AULA", "ERRO EDIT | MESSAGE : " + ex.Message);
            }
            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Excluir
        /// </summary>
        /// <param name="id">Id Status</param>
        /// <returns></returns>
        [HttpPost]
        [Permission(Permissoes.Admin)]
        public ActionResult Excluir(int id)
        {
            var result = false;
            try
            {
                var status = _repo.Listar().Where(m => m.Id == id).SingleOrDefault();
                status.DtExclusao = DateTime.Now;
                result = (_repo.Atualizar(status));
                

            }
            catch (Exception ex)
            {
              
            }

            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Métodos Privados

        /// <summary> Listar Aulas
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <param name="idEstado">Id do Estado</param>
        /// <param name="ativo">Ativo</param>
        /// <returns>PartialView</returns>
        [HttpPost]
        public ActionResult ListarStatus(string termo, int idEstado, bool ativo)
        {
            //Recupera Lista de Páginas por parâmetros
            var model = RetornaLista(termo, idEstado, ativo);

            //retorno
            return PartialView("~/Views/StatusSituacaoAula/_listarStatus.cshtml", model);
        }

        /// <summary>Retorna Lista de Status
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <param name="idEstado">IdEstado</param>
        /// <param name="ativo">Ativo</param>
        /// <returns>List StatusSituacaoAula </returns>
        private IList<StatusSituacaoAula> RetornaLista(string termo, int idEstado, bool ativo)
        {
            // variavel
            var lista = new List<StatusSituacaoAula>();

            lista = _repo.Listar().ToList();

            // Verifica o tipo de retorno
            if (!string.IsNullOrEmpty(termo))
            {
                lista = lista.Where(m => m.Nome.ToUpper().Contains(termo.ToUpper())).ToList();
            }
            
            // Id Estado
            if (idEstado > 0)
                lista = lista.Where(m => m.Estado.Id == idEstado).ToList();

            // Ativo
            if (ativo)
                lista = lista.Where(m => m.DtExclusao == null).ToList();
            else
                lista = lista.Where(m => m.DtExclusao != null).ToList();

            //Return MODEL
            return lista;
        }


        #endregion
    }
}