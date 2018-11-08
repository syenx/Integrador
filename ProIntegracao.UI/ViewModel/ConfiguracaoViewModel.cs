using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// ConfiguracaoViewModel
    /// </summary>
    public class ConfiguracaoViewModel : BaseViewModel
    {
        #region Variáveis 

        private static readonly RepositorioConfiguracao _repo = new RepositorioConfiguracao();

        #endregion

        #region Construtor

        /// <summary>
        /// ConfiguracaoViewModel
        /// </summary>
        public ConfiguracaoViewModel()
        {

        }

        /// <summary>
        /// ConfiguracaoViewModel
        /// </summary>
        /// <param name="idConfiguracao"></param>
        public ConfiguracaoViewModel(int idConfiguracao)
        {
            var config = _repo.Obter<Configuracao>(idConfiguracao);

            Id = config.Id;
            Nome            = config.Nome;
            Descricao       = config.Descricao;
            Valor           = config.Valor;
          //  DtCadastro      = config.DtCadastro;
            DtExclusao      = config.DtExclusao;
        }

        #endregion

        #region Propriedade

        /// <summary>
        /// Nome
        /// </summary>
        [DisplayName("Nome :")]
        public string Nome { get; set; }

        /// <summary>
        /// Descricao
        /// </summary>
        [DisplayName("Descrição :")]
        public string Descricao { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [DisplayName("Valor :")]
        public string Valor { get; set; }
        
        #endregion
        
        #region Metodos

        /// <summary> Parse Pagina View Model
        /// </summary>
        /// <param name="model">Pagina View Model</param>
        /// <returns>Pagina</returns>
        public Configuracao ParseConfiguracaoViewModel(ConfiguracaoViewModel model)
        {
            var config = new Configuracao();

            if (model.Id > 0)config = _repo.Obter<Configuracao>(model.Id);

            config.Id           = (config == null) ? model.Id : config.Id;
            config.Nome         = model.Nome;
            config.Descricao    = model.Descricao;
            config.Valor        = model.Valor;
          //  config.DtCadastro   = (model.Id > 0)?model.DtCadastro:DateTime.Now;
            config.DtExclusao   = model.DtExclusao;
            
            return config;
        }


        #endregion
    }
}