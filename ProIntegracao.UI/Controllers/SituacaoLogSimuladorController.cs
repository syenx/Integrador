using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;
using ProIntegracao.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Situacao Log Simulador
    /// </summary>
    public class SituacaoLogSimuladorController : BaseController
    {
        #region variáveis

        private static readonly RepositorioSituacaoLogSimulador _repo = new RepositorioSituacaoLogSimulador();

        #endregion

        #region Actions

        /// <summary>
        /// Index - GET
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/SituacaoLogSimulador");
            var viewmodel = new SituacaoLogSimuladorViewModel();
            return View(viewmodel);
        }

        /// <summary>
        /// Listar Situação
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <param name="dtInicio">Data Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="psa">PSA</param>
        /// <param name="acao">Acao do Simulador</param>
        /// <returns></returns>
        public ActionResult ListarSituacao(string cpf, string dtInicio, string dtFim, string psa, int acao)
        {
            var model = RetornaLista(cpf, dtInicio, dtFim, psa, acao);
            return PartialView("~/Views/SituacaoLogSimulador/_listarSituacaoLogSimulador.cshtml", model);
        }

        /// <summary>
        /// Retorna Lista de Log Simulador
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <param name="dtInicio">Data Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="psa">PSA</param>
        /// <param name="acao">Acao do Simulador</param>
        /// <returns></returns>
        public List<SituacaoLogSimulador> RetornaLista(string cpf, string dtInicio, string dtFim, string psa, int acao)
        {
            var lista = new List<SituacaoLogSimulador>();

            try
            {
        //       lista = _repo.ListarSituacaoLogSimulador(cpf, dtInicio, dtFim, psa, acao);
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
            }
            
            return lista;
            
        }

        #endregion

    }
}