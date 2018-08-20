using ProIntegracao.Model.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// Situação Aula View Model
    /// </summary>
    public class SituacaoAulaViewModel
    {

        #region Variaveis

        private static readonly RepositorioStatusSituacaoAula _repoStatusSituacao = new RepositorioStatusSituacaoAula();
        private static readonly RepositorioUF _repoUF = new RepositorioUF();
        private static readonly RepositorioAulaStatus _repoAulaStatus = new RepositorioAulaStatus();
        #endregion

        #region Construtor

        /// <summary>
        /// Situacao Aula View Model
        /// </summary>
        public SituacaoAulaViewModel()
        {
            ListaCursos = CarregaTipoCursos();
            ListaStatus = CarregaStatus();
            ListaEstados = CarregarEstados();

        }
        
        #endregion
        
        #region Propriedades

        /// <summary>
        /// Id Situação Aula
        /// </summary>
        public int IdSituacaoAula { get; set; }

        /// <summary>
        /// Id Estado
        /// </summary>
        public string UF { get; set; }

        /// <summary>
        /// Municipio
        /// </summary>
        public string Municipo { get; set; }

        /// <summary>
        /// CFC
        /// </summary>
        public int CFC { get; set; }

        /// <summary>
        /// Data Nascimento
        /// </summary>
        public DateTime DtNascimento { get; set; }
        
        /// <summary>
        /// Data Aula
        /// </summary>
        public DateTime DtAula { get; set; }

        /// <summary>
        /// Agendado
        /// </summary>
        public string Agendado { get; set; }

        /// <summary>
        /// Inicio
        /// </summary>
        public string Inicio { get; set; }

        /// <summary>
        /// Fim
        /// </summary>
        public string Fim { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Agenda
        /// </summary>
        public int Agenda { get; set; }

        /// <summary>
        /// Sequencia
        /// </summary>
        public string Sequencia { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        public string Modelo { get; set; }

        /// <summary>
        /// PSA
        /// </summary>
        public string PSA { get; set; }

        /// <summary>
        /// CPF Aluno
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Renach Aluno
        /// </summary>
        public string Renach { get; set; }

        /// <summary>
        /// Id Status Situacao Aula
        /// </summary>
        public int IdStatus { get; set; }

        /// <summary>
        /// Lista Status
        /// </summary>
        public List<SelectListItem> ListaStatus { get; set; }

        /// <summary>
        /// Id Tipo Curso
        /// </summary>
        public int IdTipoCurso { get; set; }
        
        /// <summary>
        /// Tipo Curso
        /// </summary>
        public string TipoCurso { get; set; }

        /// <summary>
        /// Lista Cursos
        /// </summary>
        public List<SelectListItem> ListaCursos { get; set; }

        /// <summary>
        /// Lista Estados
        /// </summary>
        public List<SelectListItem> ListaEstados { get; set; }

        

        /// <summary>
        /// ID UF
        /// </summary>
        public int IdUF { get; set; }


        #endregion

        #region Métodos

        /// <summary>
        /// Carrega Lista de Tipo de Cursos
        /// </summary>
        /// <returns>List SelectListItem</returns>
        private List<SelectListItem> CarregaTipoCursos() {

            var lista = new List<SelectListItem>();

            lista.Add(new SelectListItem()
            {
                Text = "PH"
                , Value = "1"
            });

            lista.Add(new SelectListItem()
            {
                Text = "AV"
                , Value = "2"
            });

            return lista;
        }


        /// <summary>
        /// Carrega Lista de Tipo de Cursos
        /// </summary>
        /// <returns>List SelectListItem</returns>
        private List<SelectListItem> CarregaStatus()
        {
            var lista = new List<SelectListItem>();
            var model = _repoAulaStatus.ObterTodos().OrderBy(m=>m.Id).ToList();
            
            foreach (var item in model)
            {
                lista.Add(new SelectListItem()
                {
                    Text = string.Format("{0}-{1}", item.Id, item.Nome)
                    , Value = item.Id.ToString()
                });
            }
            
            return lista;
        }
        
        /// <summary> Carregar Estados
        /// </summary>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> CarregarEstados(int id = 0)
        {
            var estado = _repoUF.ObterTodos();

            var lista = new List<SelectListItem>();

            foreach (var item in estado)
            {
                var slitem = new SelectListItem()
                {
                    Text = item.Nome,
                    Value = item.Id.ToString(),
                    Selected = (item.Id == id) ? true : false
                };
                lista.Add(slitem);
            }
            return lista;
        }

        #endregion


    }
}