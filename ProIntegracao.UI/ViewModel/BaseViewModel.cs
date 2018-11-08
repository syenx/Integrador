using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System;
using System.Linq;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// BaseViewModel
    /// </summary>
    public class BaseViewModel
    {
        #region Variaveis
   
        #endregion

        #region Propriedades

        /// <summary> Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Aluno 
        /// </summary>
        public Aluno aluno { get; set; }

        /// <summary> Data Cadastro
        /// </summary>
        public string DtCadastro { get; set; }

        /// <summary> Data Exclusao
        /// </summary>
        public DateTime? DtExclusao { get; set; }

        #endregion
        
        #region Metodos

 
        #endregion
    }
}