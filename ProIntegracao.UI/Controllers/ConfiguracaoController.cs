using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;
using ProIntegracao.UI.ViewModel;
using System.Web.Mvc;
using System.Linq;
using System;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Configuração Controller
    /// </summary>
    [Permission(Permissoes.Admin)]
    public class ConfiguracaoController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioConfiguracao _repo = new RepositorioConfiguracao();

        #endregion
        
        #region Actions

        /// <summary>INDEX
        /// </summary>
        /// <returns>View</returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/Configuracao");
            return View();
        }
        
        /// <summary> Configuracao - CREATE 
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Create()
        {
            var model = new ConfiguracaoViewModel();
            return PartialView(model);
        }

        /// <summary> Configuracao - CREATE - POST
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ConfiguracaoViewModel model)
        {
            var result = false;

            try
            {
                var config = model.ParseConfiguracaoViewModel(model);
                result = (_repo.Salvar(config) > 0);
                
            }
            catch (Exception ex)
            {
                string message = string.Format("ERRO CREATE CONFIGURAÇÃO | MENSAGEM : {0}", ex.Message);
              
            }
            
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary> Configuracao - Edit 
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Edit(int idConfiguracao)
        {
            var model = new ConfiguracaoViewModel(idConfiguracao);
            return PartialView(model);
        }

        /// <summary> Configuracao - EDIT - POST
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ConfiguracaoViewModel model)
        {
            var result = false;

            try
            {           
                var config = model.ParseConfiguracaoViewModel(model);
                result = _repo.Atualizar(config);
              

            }
            catch (Exception ex)
            {
                string message = string.Format("ERRO EDIT CONFIGURAÇÃO | MENSAGEM : {0}", ex.Message);
                //InserirLog("CONFIGURACAO", message);
            }
            
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary> Excluir Configuração
        /// </summary>
        /// <param name="id">Id da Configuracao</param>
        /// <returns>json</returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Excluir(int id)
        {
            var result = false;
            try
            {
                var config = _repo.Listar().Where(m => m.Id == id).FirstOrDefault();
                config.DtExclusao = DateTime.Now;
                result = _repo.Atualizar(config);
             
            }
            catch (Exception ex)
            {
                string message = string.Format("ERRO DELETE CONFIGURAÇÃO | MENSAGEM : {0}", ex.Message);
           
            }

            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Métodos 

        /// <summary>Listar Configuração
        /// </summary>
        /// <returns>html</returns>
        [HttpPost]
        public ActionResult ListarConfiguracao(string termo, bool ativo)
        {
            var model = _repo.Listar().ToList();

            if (ativo)
                model = model.Where(m => m.DtExclusao == null).ToList();
            else
                model = model.Where(m => m.DtExclusao != null).ToList();

            if (!string.IsNullOrEmpty(termo))
                model = model.Where(m => m.Nome.ToLower().Contains(termo.ToLower())).ToList();

            return PartialView("~/Views/Configuracao/_listarConfiguracao.cshtml", model);
        }


        /// <summary>Obter Configuracao Existente
        /// </summary>
        /// <param name="nome">Descricao</param>
        /// <param name="id">Id da Configuração</param>
        /// <returns></returns>
        public ActionResult ObterConfiguracaoExistente(string nome, int id = 0)
        {
            var result = "true";
            var config = _repo.Listar().Where(m => m.Nome.ToUpper() == nome.ToUpper()).FirstOrDefault();

            if (config != null)
            {
                if(id == 0)
                    result = "*Configuração já existe";
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        #endregion
    }
}