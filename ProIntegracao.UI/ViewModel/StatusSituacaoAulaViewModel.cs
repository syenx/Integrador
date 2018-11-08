using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// StatusSituacaoAulaViewModel
    /// </summary>
    public class StatusSituacaoAulaViewModel : BaseViewModel
    {
        #region Variaveis

        private static readonly RepositorioStatusSituacaoAula _repo = new RepositorioStatusSituacaoAula();
        private static readonly RepositorioEstado _repoEstado = new RepositorioEstado();
        
        #endregion

        #region Construtor

        /// <summary>Aula View Model
        /// </summary>
        public StatusSituacaoAulaViewModel()
        {
            ListaEstado = CarregarEstados();
        }

        /// <summary>
        /// Status Situacao Aula View Model
        /// </summary>
        /// <param name="model">Status Situacao</param>
        public StatusSituacaoAulaViewModel(StatusSituacaoAula model)
        {
            if (model.Id > 0)
            {
                model = _repo.Listar().Where(m => m.Id == model.Id).FirstOrDefault();
            }

            Id                  = model.Id;
            Identificador       = model.Identificador;
            Nome                = model.Nome;
            IdEstado            = model.Estado.Id;
            ListaEstado         = CarregarEstados();
        //    DtCadastro          = model.DtCadastro;
            DtExclusao          = model.DtExclusao;
        }

        #endregion

        #region Propriedade

        /// <summary>
        /// Nome
        /// </summary>
        [DisplayName("Nome :")]
        public string Nome { get; set; }

        /// <summary>
        /// Identificador
        /// </summary>
        [DisplayName("Identificador :")]
        public int Identificador { get; set; }

        /// <summary>
        /// Id Estado
        /// </summary>
        [DisplayName("Estado :")]
        public int IdEstado { get; set; }

        /// <summary>
        /// Lista Estado
        /// </summary>
        public List<SelectListItem> ListaEstado { get; set; }

        #endregion

        #region Métodos

        /// <summary> Carregar Estados
        /// </summary>
        /// <param name="id">Id do Estado</param>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> CarregarEstados(int id = 0)
        {
            var estado = _repoEstado.Listar().OrderBy(m => m.Nome);

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

        

        /// <summary> Parse Status Situacao View Model
        /// </summary>
        /// <param name="model">Pagina View Model</param>
        /// <returns>Pagina</returns>
        public StatusSituacaoAula ParseStatusViewModel(StatusSituacaoAulaViewModel model)
        {
            var status = new StatusSituacaoAula();

            if (model.Id > 0)status = _repo.Obter<StatusSituacaoAula>(model.Id);

            status.Identificador    = model.Identificador;
            status.Nome             = model.Nome;
            status.Estado           = _repoEstado.Obter<Estado>(model.IdEstado);
       //     status.DtCadastro       = model.DtCadastro;
            status.DtExclusao       = model.DtExclusao;
            
            return status;
        }

        #endregion
    }
}