using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data.Entidade
{
    /// <summary>
    /// Situação Aula
    /// </summary>
    public class SituacaoAula
    {

        #region Propriedades
        
        /// <summary>
        /// Id Situação Aula
        /// </summary>
        public long IdSituacaoAula { get; set; }
        
        /// <summary>
        /// Id Empresa
        /// </summary>
        public int IdEmpresa { get; set; }
        public string RazaoSocial { get; set; }

        /// <summary>
        /// Id Aula
        /// </summary>
        public long IdAula { get; set; }

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
        public string CFC { get; set; }

        /// <summary>
        /// CNPJ
        /// </summary>
        public string CNPJ { get; set; }

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
        public long Agenda { get; set; }

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
        /// SESSION ID
        /// </summary>
        public string SESSION_ID { get; set; }

        /// <summary>
        /// CPF Aluno
        /// </summary>
        public string Cpf { get; set; }


        /// <summary>
        /// CPF Aluno
        /// </summary>
        public string Nome { get; set; }
        
        /// <summary>
        /// Renach Aluno
        /// </summary>
        public string Renach { get; set; }

        /// <summary>
        /// Id Status Situacao Aula
        /// </summary>
        public int IdStatus { get; set; }
        
        /// <summary>
        /// Id Tipo Curso
        /// </summary>
        public int IdTipoCurso { get; set; }

        /// <summary>
        /// Tipo Curso
        /// </summary>
        public string TipoCurso { get; set; }
        
        /// <summary>
        /// Xml de Entrada de LOG
        /// </summary>
        public string XMLEntrada { get; set; }
        
        /// <summary>
        /// XML de Saida
        /// </summary>
        public string XMLSaida { get; set; }

        #endregion
    }
}
