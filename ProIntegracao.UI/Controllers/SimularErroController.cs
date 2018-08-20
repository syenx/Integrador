using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Simular Erro Controller
    /// </summary>
    public class SimularErroController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioSimularErro _repo = new RepositorioSimularErro();
        private static readonly RepositorioTipoErro _repoTipoErro = new RepositorioTipoErro();
        private static readonly RepositorioAluno _repoAluno = new RepositorioAluno();
        
        #endregion

        #region Actions

        /// <summary>INDEX
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/SimularErro");
            return View();
        }
        
        /// <summary> Details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int id)
        {
            return View();
        }

        /// <summary>Forçar Erro - GET
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var model = new SimularErroViewModel();
            return PartialView(model);
        }

        /// <summary> CREATE
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SimularErroViewModel model)
        {
            var resultado = false;

            try
            {
                var forcar = model.ParseForcarViewModel(model);
                forcar.DtCadastro = DateTime.Now;
                resultado = (_repo.Salvar(forcar) > 0);
            }
            catch (Exception ex)
            {
                
            }

            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Excluir Forcar Erro
        /// </summary>
        /// <param name="id">Id Simulador de Erro</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var result = false;
            try
            {
                var erro = _repo.Listar().Where(m => m.Id == id).SingleOrDefault();
                _repo.Excluir(erro);

                result = true;
            }
            catch (Exception ex)
            {
                
            }

            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Métodos
        
        /// <summary> Listar Paginas
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <returns>PartialView</returns>
        [HttpPost]
        public ActionResult ListarSimularErro(string termo)
        {
            //Recupera Lista de Páginas por parâmetros
            var model = RetornaLista(termo);

            //retorno
            return PartialView("~/Views/SimularErro/_listarSimularErro.cshtml", model);
        }
        
        /// <summary>Retorna Lista de Páginas
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <returns>IList Pagina </returns>
        private IList<ForcarErro> RetornaLista(string termo)
        {
            // variavel
            var model = new List<ForcarErro>();

            model = _repo.Listar().ToList();

            // Verifica o tipo de retorno
            if (!string.IsNullOrEmpty(termo))
                model = model.Where(m => m.Aluno.CpfAluno.Contains(termo)).ToList();

            //Return MODEL
            return model;
        }
        
        /// <summary>CPF Não Cadastrado
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <param name="id">Id do Erro</param>
        /// <returns></returns>
        public ActionResult CPFNaoCadastrado(string cpf, int id = 0)
        {
            var result = "true";

            cpf = cpf.Replace(".", "").Replace("-", "");

            var aluno = _repoAluno.Listar().Where(m => m.CpfAluno == cpf && m.DtExclusao == null).FirstOrDefault();
            
            if (aluno == null)
            {
                result = "*CPF não encontrado";
            }
            else
            {
                var erro = _repo.Listar().Where(m => m.Aluno.CpfAluno == cpf).FirstOrDefault();

                if (erro != null)
                {
                    result = "*CPF já existe";
                }
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #endregion
    }
}