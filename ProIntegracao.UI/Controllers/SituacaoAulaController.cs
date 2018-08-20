using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.ViewModel;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Situacao Aula Controller
    /// </summary>
    public class SituacaoAulaController : BaseController
    {   
        #region Variáveis

        private static readonly RepositorioSituacaoAula _repo = new RepositorioSituacaoAula();
        
        #endregion
        
        #region Actions

        /// <summary> Consulta Situação Aula
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/SituacaoAula");
            var viewmodel = new SituacaoAulaViewModel();
            return View(viewmodel);
        }
        
        #endregion

        #region Métodos

        /// <summary>
        /// Listar Situacao Aula
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <param name="idEstado">Id Estado</param>
        /// <param name="dtInicio">Dt Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="renach">Renach</param>
        /// <param name="status">Status</param>
        /// <param name="curso">Curso</param>
        /// <returns></returns>
        public  ActionResult ListarSituacaoAula(string cpf, string renach,string dtInicio,string dtFim,int status = 0,int curso = 0, int idEstado = 0)        {
            var model = RetornaLista(cpf,idEstado,renach,dtInicio,dtFim,status, curso);
            return PartialView("~/Views/SituacaoAula/_listaSituacaoAula.cshtml", model);
        }

        /// <summary>
        /// Retorna Lista
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <param name="idEstado">Id Estado</param>
        /// <param name="dtInicio">Dt Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="renach">Renach</param>
        /// <param name="status">Status</param>
        /// <param name="curso">Curso</param>
        /// <returns></returns>
        private static List<SituacaoAula> RetornaLista(string cpf, int idEstado, string renach, string dtInicio, string dtFim, int status, int curso)
        {
            var lista = new List<SituacaoAula>();

            lista = _repo.ListarSituacaoAula(cpf, idEstado, renach, dtInicio, dtFim, status, curso);

            return lista;
        }


        

       

    }

    #endregion

}