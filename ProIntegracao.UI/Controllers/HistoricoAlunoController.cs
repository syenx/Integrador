using ProIntegracao.Data.EntidadeProSimulador;
using ProIntegracao.Model.ReenvioAula;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;
using ProIntegracao.UI.Helper;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Web.Mvc;

namespace ProIntegracao.UI.Controllers
{
    /// <summary> Historico de Alunos
    /// 
    /// </summary>
    public class HistoricoAlunoController : BaseController, IDisposable
    {
        #region Variáveis

        /// <summary>Repositorio Historico Aluno
        /// </summary>
        public RepositorioHistoricoAluno _repo = new RepositorioHistoricoAluno();

        #endregion

        #region Actions
        
        /// <summary>INDEX
        /// </summary>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/HistoricoAluno");
            return View();
        }

        /// <summary> Consulta Aluno Individual
        /// </summary>
        /// <param name="idAluno"></param>
        /// <returns></returns>
        [Permission(Permissoes.Consultar)]
        public ActionResult Consulta(int idAluno)
        {
            var aluno = RetornaAluno(idAluno);
            return View(aluno);
        }
        
        /// <summary>Finalizar Aula
        /// </summary>
        /// <param name="idMatricula">Id MAtricula Aluno</param>
        /// <returns></returns>
        public ActionResult Finalizar(int idMatricula)
        {
            var model = _repo.RetornarFinalizarAula(idMatricula);
            return View("~/Views/HistoricoAluno/_finalizacao.cshtml", model);
        }
        
        #endregion

        #region Métodos

        /// <summary> Retorna Aluno Individual
        /// </summary>
        /// <param name="idAluno"></param>
        /// <returns></returns>
        public HistoricoAluno RetornaAluno(int idAluno)
        {
            var aluno = _repo.RetornaAluno(idAluno);
            return aluno;
        }
        
        /// <summary>Detalhe do ALUNO
        /// </summary>
        /// <returns></returns>
        public ActionResult DetalheAluno()
        {
            return PartialView("~/Views/HistoricoAluno/_form.cshtml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpf">CPF aluno</param>
        /// <param name="renach">RENACH</param>
        /// <param name="nome">Nome ALUNO</param>
        /// <param name="ativo">Aluno ATIVO?</param>
        /// <returns>HTML</returns>
        public ActionResult ListarHistorico(string cpf, string renach, string nome, bool ativo)
        {
            var model = RetornaLista(cpf, renach, nome, ativo);
            return PartialView("~/Views/HistoricoAluno/_listarAluno.cshtml", model);

        }

        /// <summary>Retorna Lista
        /// 
        /// </summary>
        /// <param name="cpf">CPF aluno</param>
        /// <param name="renach">RENACH</param>
        /// <param name="nome">Nome ALUNO</param>
        /// <param name="ativo">Aluno ATIVO?</param>
        /// <returns>Lista Historicos</returns>
        public List<HistoricoAluno> RetornaLista(string cpf, string renach, string nome , bool ativo)
        {
            var lista = new List<HistoricoAluno>();
            lista = _repo.ListarHistoricoAluno(cpf,renach,nome, ativo);
            return lista;
        }

        /// <summary>Retorna Detalhe
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RetornaDetalhe(int idAluno, int idAula)
        {
            var aluno = RetornaDetalheAluno(idAluno, idAula);
            return PartialView("~/Views/HistoricoAluno/_form.cshtml", aluno);
        }
        
        /// <summary> LIstar Matriculas
        /// </summary>
        /// <param name="idAluno">Id Aluno</param>
        /// <returns></returns>
        public ActionResult ListarMatriculas(int idAluno)
        {
            var lista = RetornaMatriculas(idAluno);
            return PartialView("~/Views/HistoricoAluno/_listarMatricula.cshtml", lista);
        }

        /// <summary>Listar Agenda
        /// </summary>
        /// <param name="idMatricula"></param>
        /// <returns></returns>
        public ActionResult ListarAgenda(int idMatricula)
        {
            var lista = RetornaAgenda(idMatricula);
            return PartialView("~/Views/HistoricoAluno/_listarAgenda.cshtml", lista);
        }

        /// <summary>Listar Historico Aula
        /// </summary>
        /// <param name="idMatricula"></param>
        /// <returns></returns>
        public ActionResult ListarHistoricoAula(int idMatricula)
        {
            var lista = RetornaHistorico(idMatricula);
            return PartialView("~/Views/HistoricoAluno/_listarHistoricoAula.cshtml", lista);
        }

        /// <summary> Retorna Matriculas
        /// </summary>
        /// <param name="idAluno">IdAluno</param>
        /// <returns></returns>
        public List<Matricula> RetornaMatriculas(int idAluno)
        {
            var lista = _repo.ListarMatriculas(idAluno);
            return lista;
        }
        
        /// <summary>Retorna Agenda
        /// </summary>
        /// <param name="idMatricula">Id Matricula</param>
        /// <returns></returns>
        public List<Agenda> RetornaAgenda(int idMatricula)
        {
            var lista = _repo.ListarAgendas(idMatricula);
            return lista;
        }

        /// <summary>Retorna Historico
        /// </summary>
        /// <param name="idMatricula">Id Matricula</param>
        /// <returns></returns>
        public List<HistoricoAula> RetornaHistorico(int idMatricula)
        {
            var lista = _repo.ListarHistorico(idMatricula);
            return lista;
        }
        
        /// <summary>Retorna Detalhe Aluno
        /// </summary>
        /// <param name="idAluno">id ALuno</param>
        /// <param name="idAula">id Aula</param>
        /// <returns></returns>
        public Aluno RetornaDetalheAluno(int idAluno, int idAula)
        {
            var aluno = _repo.RetornaDetalheAluno(idAluno, idAula);
            return aluno;
        }

        /// <summary>Retorna Status Aula PRO
        /// </summary>
        /// <returns></returns>
        public ActionResult RetornaStatusAulaPro()
        {
            var lista = EnumHelper.GetItems<StatusAula>();
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Retorna Lista Modelo PRO'
        /// </summary>
        /// <returns>json</returns>
        public ActionResult RetornaListaModeloPro(int idSimulador)
        {
            var lista = _repo.RetornaListaModeloAula(idSimulador);
            return Json(new { Resultado = lista }, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>Inserir Aula Reenvio
        /// </summary>
        /// <returns></returns>
        public ActionResult EnviarAula(int idAgenda)
        {
            var result = _repo.EnviarAula(idAgenda);
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Cancelar Aula
        /// </summary>
        /// <param name="idAgenda">Id Agenda</param>
        /// <returns></returns>
        public ActionResult CancelarAula(int idAgenda)
        {
            var result = _repo.CancelarAula(idAgenda);
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>Alterar Status Modelo Aula
        /// 
        /// </summary>
        /// <param name="idAula">Id Aula</param>
        /// <param name="novoIdStatus">Id Status</param>
        /// <param name="novoIdModelo">Id Modelo</param>
        /// <returns></returns>
        public ActionResult AlterarStatusModeloAula(int idAula, int novoIdStatus, int novoIdModelo)
        {
            var result = _repo.AlterarStatusModeloAula(idAula, novoIdStatus, novoIdModelo);
            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary> Consulta DETRAN
        /// 
        /// </summary>
        /// <param name="nome">Nome do Aluno</param>
        /// <param name="cpf">CPF do Aluno</param>
        /// <param name="identificadoraula">Identificador da AULA</param>
        /// <returns></returns>
        public ActionResult ConsultaDetran(string nome, string cpf, long identificadoraula)
        {
            var model = _repo.ConsultaDetran(cpf, identificadoraula);
            model.Nome = nome;
            return PartialView("~/Views/HistoricoAluno/_ecnh.cshtml", model);
        }

        /// <summary>
        /// Listar Eventos
        /// </summary>
        /// <param name="idAula">Id Aula</param>
        /// <param name="idModelo">Id Modelo</param>
        /// <param name="collapse">Collapse</param>
        /// <returns>Partial View</returns>
        public ActionResult ListarEventos(int idAula, int idModelo, string collapse)
        {
            var model = _repo.ListarEventos(idModelo);

            ViewBag.IdAula = idAula;
            ViewBag.IdModelo = idModelo;
            ViewBag.Collapse = collapse;

            return PartialView("~/Views/HistoricoAluno/_listaEventos.cshtml", model);
        }

        /// <summary>
        /// Finalizar Aula Modelo
        /// </summary>
        /// <param name="listAulas">Lista de Historico Aula</param>
        /// <returns></returns>
        public ActionResult FinalizarAulaModelo(List<FinalizarAula> listAulas)
        {
            var result = AtualizaFinalizacaodeAula(listAulas);
            
            return Json (new { Resultado = result}, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Atualizar Finalização de Aula
        /// </summary>
        /// <param name="listaAulas">Lista de Aulas</param>
        /// <returns></returns>
        public bool AtualizaFinalizacaodeAula(List<FinalizarAula> listaAulas)
        {
            var result = false;

            try
            {
                result = _repo.AtualizarFinalizacaodeAula(listaAulas);

                if (result)
                {
                    foreach (var item in listaAulas)
                    {
                        using (ReenvioAulaAutomaticoClient servico = new ReenvioAulaAutomaticoClient())
                        {
                            bool retornoservico = servico.EnvioAulaIntegracao(Convert.ToInt32(item.ID_AULA));
                            
                            //Caso não foi enviado para Serviço, deixar na fila para reenvio
                            if (!retornoservico) { 
                                var resultado = _repo.AtualizarAgenda(Convert.ToInt32(item.ID_AULA));
                            }
                        };
                    }
                }
            }
            catch (FaultException ex)
            {
                var msgErro = ex.Message;
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
            }
            
            return result;
        }



        #endregion
    }
}