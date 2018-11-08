using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;
using ProIntegracao.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>Matricula Controlller
    /// </summary>
    
    public class MatriculaController : BaseController
    {
       
        private static readonly RepositorioMatricula _repo = new RepositorioMatricula();
        private static readonly RepositorioStatusSituacaoAula _repoStatus = new RepositorioStatusSituacaoAula();
        private static readonly RepositorioAula _repoAula = new RepositorioAula();
        private static readonly RepositorioAluno _repoAluno = new RepositorioAluno();
         

        /// <summary> Index
        /// 
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/Matricula");
            return View();
        }

        /// <summary> CREATE Matricula - GET
        /// </summary>
        /// <returns>View</returns>
        [Permission(Permissoes.Inserir)]
        public ActionResult Create()
        {
            var model = new MatriculaViewModel();
            return PartialView(model);
        }

        /// <summary>Create Matricula - POST
        /// </summary>
        /// <param name="model">Matricula View Model</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult Create(MatriculaViewModel model)
        {
            var result = false;

            try
            {
                var matricula = model.ParseMatriculaViewModel(model);
                result = (_repo.Salvar(matricula) > 0);
                CadastrarAulas(model);

            
            }
            catch (Exception ex)
            {
                var message = string.Format("ERRO CREATE MATRICULA | MENSAGEM : {0}", ex.Message);
              
            }

            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);

        }

        /// <summary> EDIT Matricula - GET
        /// </summary>
        /// <param name="Id">Matricula ID</param>
        /// <returns>View</returns>
        [Permission(Permissoes.Atualizar)]
        public ActionResult Edit(int Id)
        {
            var matricula = _repo.Listar().Where(m => m.Id == Id).FirstOrDefault();
            var model = new MatriculaViewModel(matricula);
            
            return PartialView("Edit", model);
        }

        /// <summary> EDIT Matricula - POST
        /// </summary>
        /// <param name="model">Matricula View Model</param>
        /// <returns>json</returns>
        [HttpPost]
        public ActionResult Edit(MatriculaViewModel model)
        {
            var result = false;
            try
            {
                var matricula = model.ParseMatriculaViewModel(model);
                result = _repo.Atualizar(matricula);
              
            }
            catch (Exception ex)
            {
                var message = string.Format("ERRO EDIT MATRICULA | MENSAGEM : {0}", ex.Message);
             
            }
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary> DELETE Matricula
        /// 
        /// </summary>
        /// <param name="id">ID Matricula</param>
        /// <returns>json</returns>
        [Permission(Permissoes.Excluir)]
        [HttpPost]
        public ActionResult Excluir(int id)
        {
            var result = false;

            try
            {
                var matricula = _repo.Listar().Where(m => m.Id == id).FirstOrDefault();
                matricula.DtExclusao = DateTime.Now;
                _repo.Excluir(matricula);
                result = true;
                
            }
            catch (Exception ex)
            {
                var message = string.Format("ERRO DELETE MATRICULA | MENSAGEM : {0}", ex.Message);
            
            }
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

 

        #region Métodos

        /// <summary>Listar Matriculas
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <returns></returns>
        public ActionResult ListarMatriculas(string termo)
        {
            //Recupera Lista de Usuários por parâmetros
            var model = RetornaLista(termo);

            //retorno
            return PartialView("~/Views/Matricula/_listaMatricula.cshtml", model);

        }

        /// <summary>Retorna Lista Com Parâmetos
        /// </summary>
        /// <param name="termo">Termo de Busca</param>
        /// <returns>IList Matricula </returns>
        public IList<Matricula> RetornaLista(string termo)
        {
            // variavel
            var model = new List<Matricula>();

            if (!string.IsNullOrEmpty(termo)) termo = termo.Replace(".", "").Replace("-", "");

            // Verifica o tipo de retorno
            if (!string.IsNullOrEmpty(termo))
            {
                model = _repo.Listar().Where(m => m.Aluno.CpfAluno.Contains(termo)).ToList();
            }
            else {
                model = _repo.Listar().Where(m => m.DtExclusao == null).ToList();
            }

            //retorno
            return model.OrderBy(x => x.Aluno.Nome).Where(m => m.DtExclusao == null).ToList();
        }

        /// <summary> Cadastrar Aulas
        /// QUANDO O ESTADO FOR SAO PAULO. 
        /// CADASTRA DE A CORDO COM A QUANTIDADE DE AULA INFORMADO EM MATRICULA 
        /// </summary>
        /// <param name="matricula">Matricula View Model</param>
        public void CadastrarAulas(MatriculaViewModel matricula)
        {
            try
            {
                var aluno = _repoAluno.Listar().Where(x => x.Id == matricula.IdAluno).FirstOrDefault();
                var novaMatricula = _repo.Listar().Where(x => x.Aluno.CpfAluno == aluno.CpfAluno).FirstOrDefault();

                if (novaMatricula != null)
                {
                    if (novaMatricula.Estado.Uf == "SP")
                    {
                        var count = matricula.QtdAula;

                        for (int i = 0; i < count; i++)
                        {
                            // Status Determinado para ajuste de Nova Aula
                            var statusProcura = Convert.ToInt32(ConfigurationManager.AppSettings["StatusAjusteAula"].ToString());

                            //Variaveis
                            var statusAula = _repoStatus.Obter<StatusSituacaoAula>(statusProcura);
                            var guidId = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8).ToUpper();
                            var guidTKIn = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8).ToUpper();
                            var guidTKFn = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(1, 8).ToUpper();
                            var horaCaulculada = DateTime.Parse(matricula.HoraAula).AddMinutes(i * 30);
                            var alunos = new Aluno();

                            var aula = new Aula()
                            {
                                CpfInstrutor        = "99999999999",
                                CodigoCfc           = matricula.CodigoCfc.ToString(),
                                DataInicioAula      = horaCaulculada,
                                DataFimAula         = null,
                                DtCadastro          = DateTime.Now,
                                DtExclusao          = null,
                                IdentificadorAula   = guidId,
                                Matricula           = novaMatricula,
                                StatusSituacaoAula  = statusAula,
                                TokenFimAula        = guidTKIn,
                                TokenInicioAula     = guidTKFn
                            };

                            if (_repoAula.Salvar(aula) == 0)
                            {
                                
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
            }

        }
        
        /// <summary>
        /// Retorna Lista Json
        /// </summary>
        /// <param name="termo">Termo Busca</param>
        /// <returns></returns>
        public ActionResult RetornaListaJson(string termo)
        {
            // variavel
            var model = new List<Matricula>();

            if (!string.IsNullOrEmpty(termo)) termo = termo.Replace(".", "").Replace("-", "");

            // Verifica o tipo de retorno
            if (!string.IsNullOrEmpty(termo))
            {
                model = _repo.Listar().Where(m => m.Aluno.Nome.Contains(termo) && m.DtExclusao == null).ToList();
            }
            else {
                model = _repo.Listar().Where(m => m.DtExclusao == null).ToList();
            }

            //retorno
            return Json(new
            {
                Resultado = model
                                .Where(m => m.DtExclusao == null)
                                .OrderBy(x => x.Aluno.Nome)
                                .Select(m => m.Aluno.Nome).ToList()
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retorna Lista Json
        /// </summary>
        /// <param name="cpf">Termo Busca</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidaCpfExistente(string cpf)
        {
            // variavel
            var resultado = "true";


            cpf = cpf.Replace(".", "").Replace("-", "");

            if (string.IsNullOrEmpty(cpf)) return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);

            cpf = cpf.Replace(".", "").Replace(".", "").Replace("-", "");

            // Verifica o tipo de retorno
            var model = _repoAluno.Listar().Where(m => m.CpfAluno == cpf).SingleOrDefault();

            if (model != null)
            {
                if (model.Id == 0) {
                    resultado = "*CPF já existe";
                }
            }

            //retorno
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Valida Estado Possui Matriculas
        /// </summary>
        /// <param name="idEstado"></param>
        /// <returns></returns>
        public ActionResult ValidaEstadoPossuiMatriculas(int idEstado)
        {
            List<Matricula> matriculas = new List<Matricula>();
            List<StatusSituacaoAula> status = new List<StatusSituacaoAula>();
            var result = false;
            var verificaStatus = false;

            try
            {
                matriculas = _repo.Listar().Where(m => m.DtExclusao == null && m.Estado.Id == idEstado).OrderBy(m => m.Id).ToList();

                if (matriculas.Count() > 0)
                {
                    var idEstadosStatus = ConfigurationManager.AppSettings["EstadoStatus"].ToString().Split(',');
                    foreach (var item in idEstadosStatus)
                    {
                        if (idEstado == Convert.ToInt32(item))
                        {
                            verificaStatus = true;
                            break;
                        }
                    }

                    if (verificaStatus){
                        var listaEstados = matriculas.Select(m => m.Estado.Id).Distinct().ToArray();
                        status = _repoStatus.Listar().Where(a => listaEstados.Contains(a.Estado.Id)).ToList();
                        if (status.Count() > 0) result = true;
                    }
                    else {
                        result = true;
                    }
                }

            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
            }

            return Json (new { Resultado = result},JsonRequestBehavior.AllowGet);
        }
        
        #endregion
    }
}
