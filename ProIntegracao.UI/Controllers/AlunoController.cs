using ProIntegracao.Data.Entidade;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.ViewModel;
using System;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;

namespace ProIntegracao.UI.Controllers
{
    /// <summary> Aluno Controller
    /// </summary>
    public class AlunoController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioPessoa _repo = new RepositorioPessoa();
        private static readonly RepositorioAluno _repoAluno = new RepositorioAluno();

        #endregion
        
        #region Alunos Duplicados
        
        /// <summary>
        /// INDEX - > Listagem 
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Duplicado()
        {
            ViewBag.Title = RetornaNomePagina("/Aluno/Duplicado");
            return View("IndexAlunoDuplicado");
        }

        /// <summary> Listar Alunos Duplicados
        /// </summary>
        /// <param name="cpf">cpf</param>
        /// <param name="nome">nome</param>
        /// <param name="renach">renach</param>
        /// <returns></returns>
        public ActionResult ListarAlunosDuplicados(string cpf, string nome, string renach)
        {
            var model = RetornaLista(cpf, nome, renach);
            return PartialView("~/Views/Aluno/_listarAlunosDuplicados.cshtml", model);
        }

        #endregion

        #region Actions Alunos

        /// <summary>Index
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/Aluno");
            return View();
        }
        
        /// <summary>
        /// Listar Alunos
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public ActionResult ListarAlunos(string cpf)
        {
            var model = RetornaListaIntegrados(cpf);
            return PartialView("~/Views/Aluno/_listarAlunosIntegracao.cshtml", model);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Inserir)]
        public ActionResult Create()
        {
            var model = new AlunoViewModel();
            return PartialView(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="AlunoViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(AlunoViewModel AlunoViewModel)
        {
            var resultado = false;

            try
            {
                Aluno aluno = AlunoViewModel.ParseAlunoViewModel(AlunoViewModel);
                resultado = (_repoAluno.Salvar(aluno) > 0);
                //InserirLog("ALUNO", "CREATE", aluno.CpfAluno, 0, aluno.Id, 0, 0);

            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("Aluno ", ex.Message);
            }

            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="Id">Id Aula</param>
        /// <returns></returns>
        [Permission(Permissoes.Atualizar)]
        public ActionResult Edit(int Id)
        {
            var aluno = _repoAluno.Listar().Where(m => m.Id == Id).FirstOrDefault();
            var model = new AlunoViewModel(aluno);
            return PartialView("Edit", model);
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="AlunoViewModel">Aula View Model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(AlunoViewModel AlunoViewModel)
        {
            var resultado = false;
            try
            {
                var aluno = AlunoViewModel.ParseAlunoViewModel(AlunoViewModel);
                resultado = _repoAluno.Atualizar(aluno);
                //InserirLog("ALUNO", "EDIT", aluno.CpfAluno, 0, aluno.Id, 0, 0);
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("Aluno", ex.Message);
            }
            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Excluir Registro 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Permission(Permissoes.Excluir)]
        public ActionResult Excluir(int id)
        {
            var result = false;
            try
            {
                var aluno = _repoAluno.Listar().Where(m => m.Id == id).SingleOrDefault();
                aluno.DtExclusao = DateTime.Now;
                _repoAluno.Excluir(aluno);
                result = true;
                //InserirLog("ALUNO", "DELETE", aluno.CpfAluno, 0, aluno.Id, 0, 0);
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("ALUNO", "Erro Excluir registro | Message :" + ex.Message);
            }
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Métodos


        /// <summary>
        /// Retorna Lista
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <param name="nome">Nome</param>
        /// <param name="renach">Renach</param>
        /// <returns></returns>
        private List<Data.EntidadeProSimulador.Pessoa> RetornaLista(string cpf, string nome, string renach)
        {
            var lista = new List<Data.EntidadeProSimulador.Pessoa>();
            lista = _repo.ListarAlunosDuplicados(cpf, nome, renach).ToList();
            return lista;
        }

        /// <summary> Retorna Lista Integrados
        /// </summary>
        /// <param name="cpf">Cpf Aluno</param>
        /// <returns></returns>
        private List<Aluno> RetornaListaIntegrados(string cpf)
        {

            List<Aluno> lista = null;
            lista = _repoAluno.Listar().ToList();

            if (!string.IsNullOrEmpty(cpf))
                lista = lista.Where(x => x.CpfAluno == cpf.Replace(".", "").Replace("-","")).ToList();


            return lista;
        }
        
        /// <summary>
        /// Retorna Nome por CPF
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public ActionResult RetornaNomePorCpf(string cpf)
        {
            var aluno = new Aluno() { Nome = "", Id = 0};

            try
            {
                aluno = _repoAluno.Listar().Where(m => m.CpfAluno == cpf).FirstOrDefault();

                if (aluno == null)
                {
                    aluno = new Aluno
                    {
                        Nome = string.Empty,
                        Id = 0
                    };
                }
                
            }
            catch (Exception)
            {
                //InserirLog("ALUNO", "Erro ao Selecionar Aluno Por CPF");
            }

            


            return Json(new { NomeAluno = aluno.Nome, IdAluno = aluno.Id }, JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}