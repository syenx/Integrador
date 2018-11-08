using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ProIntegracao.UI.ViewModel;
using System;
using ProIntegracao.UI.Enum;
using ProIntegracao.UI.Attribute;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Aula Controller
    /// </summary>
    public class AulaController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioAula         _repo           = new RepositorioAula();
        private static readonly RepositorioMatricula    _repoMatricula  = new RepositorioMatricula();
        private static readonly RepositorioAluno        _repoAluno      = new RepositorioAluno();
       
        #endregion

        #region Actions

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/Aula");
            var model = new AulaViewModel();
            return View(model);
        }

        /// <summary>
        ///  Create
        /// </summary>
        /// <param name="idEstado">Id do Estado</param>
        /// <returns></returns>
        [Permission(Permissoes.Inserir)]
        public ActionResult Create(int idEstado = 0)
        {
            var model = new AulaViewModel(idEstado);
            return PartialView(model);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="AulaViewModel">Aula View Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(AulaViewModel AulaViewModel)
        {
            var resultado = false;
            try
            {
                var aula = AulaViewModel.ParseAulaViewModel(AulaViewModel);
                if (aula != null)
                {
                    var teste  = _repo.Listar().Count(x => x.Id <= 5);
                   // AulaViewModel.DtCadastro = DateTime.Now;
                    resultado = (_repo.Salvar(aula) > 0);
                    //InserirLog("AULA", "CREATE", aula.Matricula.Aluno.CpfAluno, aula.Id, aula.Matricula.Aluno.Id,0, aula.Matricula.Id);
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("Pagina", ex.Message);
            }

            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="idAula">Id Aula</param>
        /// <returns></returns>
        [Permission(Permissoes.Atualizar)]
        public ActionResult Edit(int idAula)
        {
            var aula = _repo.Listar().Where(m => m.Id == idAula).FirstOrDefault();
            var model = new AulaViewModel(aula);
            return PartialView(model);
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="AulaViewModel">Aula View Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(AulaViewModel AulaViewModel)
        {
            var resultado = false;

            try
            {
                var aula = AulaViewModel.ParseAulaViewModel(AulaViewModel);
                resultado = _repo.Atualizar(aula);
                //InserirLog("AULA", "EDIT", aula.Matricula.Aluno.CpfAluno, aula.Id, aula.Matricula.Aluno.Id, 0, aula.Matricula.Id);
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
              
                //InserirLog("AULA", ex.Message);
            }
            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Excluir
        /// </summary>
        /// <param name="id">Id Aula</param>
        /// <returns></returns>
        [Permission(Permissoes.Excluir)]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var result = false;
            try
            {
                var aula = _repo.Listar().Where(m => m.Id == id).SingleOrDefault();
                aula.DtExclusao = DateTime.Now;
                result = (_repo.Atualizar(aula));
                //InserirLog("AULA", "DELETE", aula.Matricula.Aluno.CpfAluno, aula.Id, aula.Matricula.Aluno.Id, 0, aula.Matricula.Id);
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("AULA", ex.Message);
            }

            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Métodos Privados

        /// <summary> Listar Aulas
        /// </summary>
        /// <param name="cpf">Termo de Busca</param>
        /// <param name="nomeAluno">Nome Matricula</param>
        /// <param name="ativo">ativo</param>
        /// <returns>PartialView</returns>
        [HttpPost]
        public ActionResult ListarAulas(string cpf,string nomeAluno, bool ativo)
        {
            //Recupera Lista de Páginas por parâmetros
            var model = RetornaLista(cpf, nomeAluno, ativo);

            //retorno
            return PartialView("~/Views/Aula/_listarAulas.cshtml", model);
        }
        
        /// <summary>Retorna Lista de Páginas
        /// </summary>
        /// <param name="cpf">Cpf do Aluno</param>
        /// <param name="nomeAluno">Nome Aluno</param>
        /// <param name="ativo">Ativo</param>
        /// <returns>List Aulas </returns>
        private IList<Aula> RetornaLista(string cpf, string nomeAluno, bool ativo)
        {
            // variavel
            var model = new List<Aula>();

            // Retorna dados do repositorio
            model = _repo.Listar().ToList();
            
            //Verifica CPF
            if (!string.IsNullOrEmpty(cpf))
                model = model.Where(m => m.Matricula.Aluno.CpfAluno.Replace(".", "").Replace("-", "").Equals(cpf)).ToList();

            //Verifica NOME ALUNO
            if (!string.IsNullOrEmpty(nomeAluno))
                model = model.Where(m => m.Matricula.Aluno.Nome.ToUpper().Contains(nomeAluno.ToUpper())).ToList();
            
            //Verifica Atino Inativo
            if (!ativo)
                model = model.Where(m => m.DtExclusao != null).ToList();
            else
                model = model.Where(m => m.DtExclusao == null).ToList();

            //Return MODEL
            return model;
        }

        /// <summary>Retorna CPF por Id Matricula para Facilitar Complemento de Formulário
        /// </summary>
        /// <param name="idMatricula">Id Matricula</param>
        /// <param name="IdAula">Id Aula</param>
        /// <returns>json</returns>
        public ActionResult RetornaCPF(int idMatricula, int IdAula)
        {
            var matricula = _repoMatricula.Obter<Matricula>(idMatricula);

            var cpf = string.Empty;
            var guidId = string.Empty;
            var guidTKIn = string.Empty;
            var guidTKFn = string.Empty;

            if (matricula != null)
                cpf = matricula.Aluno.CpfAluno;

            if (IdAula > 0)
            {
                var obterDadosAula = _repo.Listar().Where(x => x.Id == IdAula).FirstOrDefault();
                guidId = obterDadosAula.IdentificadorAula;
                guidTKIn = obterDadosAula.TokenInicioAula;
                guidTKFn = obterDadosAula.TokenFimAula;
            }
            else
            {
                guidId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8);
                guidTKIn = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8);
                guidTKFn = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8);
            }

            var IdAluno = (matricula != null) ? matricula.Id : 0;
            var NomeAluno = (matricula != null) ? matricula.Aluno.Nome : "";
            var CodigoCfc = (matricula != null) ? matricula.CodigoCfc : 0;
            
            return Json(new { Cpf = cpf
                , Identificador = guidId
                , TokenInicial = guidTKIn
                , TokenFinal = guidTKIn
                , idAluno = IdAluno 
                , NomeAluno = NomeAluno
                , CodigoCfc = CodigoCfc
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary> Verifica Matricula Por Horario
        /// </summary>
        /// <param name="idMatricula">Id Matricula</param>
        /// <param name="DataInicioAula">Hora Aula</param>
        /// <param name="idAula">Id Aula</param>
        /// <returns></returns>
        public ActionResult VerificaMatriculaPorHorario(int idMatricula, string DataInicioAula, int idAula = 0)
        {
            var result = "true";

            try
            {
                if (idAula > 0) return Json(result, JsonRequestBehavior.AllowGet);

                DateTime? dataInicio = Convert.ToDateTime(DataInicioAula);

                var aula = _repo.Listar().Where(m => m.Matricula.Id == idMatricula && m.DataInicioAula.Equals(dataInicio)).FirstOrDefault();

                if (aula != null) result = "*Aula já existe.";

            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                result = "*Aula já existe.";
            }
            
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
