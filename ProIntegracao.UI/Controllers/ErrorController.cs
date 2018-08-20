using System.Web.Mvc;
using System.Web.UI;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>Error Controller
    /// </summary>
    [HandleError]
    [OutputCache(Location = OutputCacheLocation.None)]
    public class ErrorController : Controller
    {
        /// <summary>Não Autorizado
        /// </summary>
        /// <returns></returns>
        public ActionResult NaoAutorizado()
        {
            return View();
        }

        /// <summary>Http Error 404
        /// </summary>
        /// <param name="message">Mensagem de Erro</param>
        /// <returns></returns>
        public ActionResult HttpError404(string message)
        {
            return View(message);
        }

        /// <summary>Http Erro 500
        /// </summary>
        /// <param name="message">Mensagem</param>
        /// <returns></returns>
        public ActionResult HttpError500(string message)
        {
            return View(message);
        }
        
        /// <summary>Error General
        /// </summary>
        /// <param name="message">Mensagem</param>
        /// <returns></returns>
        public ActionResult General(string message)
        {
            return View(message);
        }
    }
}