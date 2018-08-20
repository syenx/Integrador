using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// AulaViewModel
    /// </summary>
    public class AulaViewModel : BaseViewModel
    {
        #region Variaveis

        private static readonly RepositorioAula _repo = new RepositorioAula();
        private static readonly RepositorioMatricula _repoMatricula = new RepositorioMatricula();
        private static readonly RepositorioStatusSituacaoAula _repoStatus = new RepositorioStatusSituacaoAula();
        private static readonly RepositorioAluno _repoAluno = new RepositorioAluno();
        private static readonly RepositorioConfiguracao _repoConfig = new RepositorioConfiguracao();


        #endregion

        #region Construtor

        /// <summary>Aula View Model
        /// </summary>
        public AulaViewModel()
        {
            ListaStatusSituacaoAula = CarregarStatusSituacaoAula();
            ListaMatriculas = CarregarMatriculas();
            EditarStatus = false;
        }

        /// <summary>
        /// AulaViewModel
        /// </summary>
        /// <param name="IdEstado"></param>
        public AulaViewModel(int IdEstado = 0)
        {
            ListaStatusSituacaoAula = CarregarStatusSituacaoAula(IdEstado);
            ListaMatriculas = CarregarMatriculas(IdEstado);
            EditarStatus = VerificaEdicaoStatus(IdEstado);
        }

        /// <summary>Verifica Edição Status
        /// </summary>
        /// <param name="idEstado">Id do Estado</param>
        /// <returns></returns>
        private bool VerificaEdicaoStatus(int idEstado)
        {
            var result = false;

            var chaves = _repoConfig.Listar().Where(x => x.Nome == "ESTADOSSTATUS").FirstOrDefault().Valor.Split(',');
            
            foreach (var item in chaves)
            {
                if (idEstado.ToString() == item)
                {
                    result = true;
                    break;
                }
            }
            
            return result;
        }

        /// <summary> Aula View Model
        /// </summary>
        /// <param name="aula">Entidade AULA</param>
        /// <param name="idEstado">Id do Estado</param>
        public AulaViewModel(Aula aula, int idEstado = 0)
        {

            if (aula.Id > 0)
            {
                aula = _repo.Obter<Aula>(aula.Id);
            }

            Id = aula.Id;
            IdAluno = aula.Matricula.Aluno.Id;
            NomeAluno = aula.Matricula.Aluno.Nome;
            CodigoCfc = aula.CodigoCfc;
            IdentificadorAula = aula.IdentificadorAula;
            CpfInstrutor = aula.CpfInstrutor;
            DataInicioAula = aula.DataInicioAula;
            DataFimAula = aula.DataFimAula;
            TokenInicioAula = aula.TokenInicioAula;
            TokenFimAula = aula.TokenFimAula;
            IdMatricula = aula.Matricula.Id;
            DtCadastro = aula.DtCadastro;
            DtExclusao = aula.DtExclusao;
            EditarStatus = VerificaEdicaoStatus(aula.Matricula.Estado.Id);

            if (aula.StatusSituacaoAula != null) {
                IdStatusSituacaoAula = aula.StatusSituacaoAula.Id;
            }

            ListaMatriculas = CarregarMatriculas(idEstado);
            ListaStatusSituacaoAula = CarregarStatusSituacaoAula(idEstado);
        }

        #endregion

        #region Propriedade

        /// <summary>
        /// CodigoCfc
        /// </summary>
        [DisplayName("Código CFC :")]
        public string CodigoCfc { get; set; }

        /// <summary>
        /// IdentificadorAula
        /// </summary>
        [DisplayName("Identificador Aula :")]
        public string IdentificadorAula { get; set; }

        /// <summary>
        /// Id do Aluno
        /// </summary>
        public int IdAluno { get; set; }

        /// <summary>
        /// CpfInstrutor
        /// </summary>
        [DisplayName("CPF Instrutor :")]
        public string CpfInstrutor { get; set; }

        /// <summary>
        /// Nome Aluno
        /// </summary>
        [DisplayName("Nome Aluno :")]
        public string NomeAluno { get; set; }

        /// <summary>
        /// DataInicioAula
        /// </summary>
        [DisplayName("Data Início :")]
        public DateTime? DataInicioAula { get; set; }

        /// <summary>
        /// DataFimAula
        /// </summary>
        [DisplayName("Data Fim :")]
        public DateTime? DataFimAula { get; set; }

        /// <summary>
        /// TokenInicioAula
        /// </summary>
        [DisplayName("Token Início Aula :")]
        public string TokenInicioAula { get; set; }

        /// <summary>
        /// TokenFimAula
        /// </summary>
        [DisplayName("Token Fim Aula :")]
        public string TokenFimAula { get; set; }

        /// <summary>
        /// IdMatricula
        /// </summary>
        public int IdMatricula { get; set; }

        /// <summary>
        /// IdStatusSituacaoAula
        /// </summary>
        public int? IdStatusSituacaoAula { get; set; }
        
        /// <summary>
        /// Lista de Matrículas
        /// </summary>
        public List<SelectListItem> ListaMatriculas { get; set; }

        /// <summary>
        /// Lista Status Situacao Aula
        /// </summary>
        public List<SelectListItem> ListaStatusSituacaoAula { get; set; }
        
        /// <summary>
        /// Pode editar ou não status a partir do IdEstado
        /// </summary>
        public bool EditarStatus { get; set; }

        #endregion

        #region Métodos
        
        /// <summary>
        /// Carregar Matriculas Válidas
        /// </summary>
        /// <param name="idEstado">Id Estado</param>
        /// <param name="id">Id Matricula</param>
        /// <returns></returns>
        public List<SelectListItem> CarregarMatriculas(int idEstado = 0, int id = 0)
        {
            List<Matricula> matriculas = new List<Matricula>();

            matriculas = _repoMatricula.Listar().Where(m=> m.DtExclusao == null).OrderBy(m => m.Id).ToList();

            if (idEstado > 0)matriculas = matriculas.Where(m => m.Estado.Id == idEstado).ToList();
            
            var lista = new List<SelectListItem>();

            foreach (var item in matriculas)
            {
                var slitem = new SelectListItem()
                {
                    Text = string.Format("Matricula : {0}", item.Id)
                    , Value = item.Id.ToString()
                    , Selected = (item.Id == id) ? true : false
                };

                lista.Add(slitem);
            }

            return lista;
        }

        /// <summary> Carrega a lista de Status por Estados
        /// </summary>
        /// <param name="idEstado">Id do Estado = 0</param>
        /// <returns>List SelectListItem </returns>
        public List<SelectListItem> CarregarStatusSituacaoAula(int idEstado = 0)
        {
            var status = _repoStatus.Listar();

            if (idEstado > 0) status = status.Where(m => m.Estado.Id == idEstado).ToList();

            var lista = new List<SelectListItem>();

            foreach (var item in status)
            {
                var slitem = new SelectListItem()
                {
                    Text = string.Format("{0}-{1}", item.Identificador, item.Nome),
                    Value = item.Id.ToString(),
                    Selected = (item.Id == idEstado) ? true : false
                };

                lista.Add(slitem);
            }
            return lista.OrderBy(m => m.Text).ToList();
        }

        /// <summary> Parse Pagina View Model
        /// </summary>
        /// <param name="model">Pagina View Model</param>
        /// <returns>Pagina</returns>
        public Aula ParseAulaViewModel(AulaViewModel model)
        {
            try
            {
                var matricula = _repoMatricula.Obter<Matricula>(model.IdMatricula);

                if (matricula != null)
                {
                    var aula = new Aula()
                    {
                        Id = model.Id,
                        Matricula           = matricula,
                        IdentificadorAula   = model.IdentificadorAula,
                        StatusSituacaoAula  = _repoStatus.Listar().Where(x => x.Id == model.IdStatusSituacaoAula).FirstOrDefault(),
                        TokenFimAula        = model.TokenFimAula,
                        TokenInicioAula     = model.TokenInicioAula,
                        CodigoCfc           = model.CodigoCfc,
                        CpfInstrutor        = model.CpfInstrutor,
                        DataInicioAula      = (model.DataInicioAula),
                        DtCadastro = DateTime.Now,
                        DtExclusao = model.DtExclusao
                    };
                    
                    if (model.DataFimAula != null)
                        aula.DataFimAula = Convert.ToDateTime(model.DataFimAula);
                    else
                        aula.DataFimAula = null;

                    return aula;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                var mensagem = ex.Message;
                throw;
            }
        }

        #endregion
    }
}