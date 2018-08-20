using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary> Situacao Log Aula
    /// </summary>
    public class SituacaoLogAulaController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioSituacaoAula _repo = new RepositorioSituacaoAula();

        #endregion

        #region Actions

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/SituacaoLogAula");
            return View();
        }
        
     
        
        #endregion
    }
}