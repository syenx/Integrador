using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>Estado Controller
    /// </summary>
    public class EstadoController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioEstado _repo = new RepositorioEstado();

        #endregion

        #region Actions

        /// <summary>Listar Estados por Usuario
        /// </summary>
        /// <returns>View</returns>
        public ActionResult ListarEstados()
        {
            var model = new List<Estado>();

            if (User == null) return RedirectToAction("Index", "Login");
            
            if (User.Usuario.Perfil.Admin)
            {
                model = _repo.Listar().OrderBy(m => m.Nome).ToList();
            }
            else
            {
                model = _repo.ConsultarEstadoPorIdUsuario(User.Usuario).DistinctBy(m => m.Uf).ToList();
            }

            return PartialView(model);
        }
        
        #endregion
    }
}