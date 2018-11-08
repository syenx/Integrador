using ProIntegracao.Model.Repositorio;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// Aula Realizada View Model
    /// </summary>
    public class AulaRealizadaViewModel
    {
        #region Variáveis Públicas

        private static readonly RepositorioEstado _repoEstado = new RepositorioEstado();
        
        #endregion
        

        #region Construtor

        /// <summary>
        /// Construtor
        /// </summary>
        public AulaRealizadaViewModel()
        {
            ListaEstados = CarregarEstados();
        }

        #endregion


        #region Propriedades

        /// <summary>
        /// Lista de Estados
        /// </summary>
        public List<SelectListItem> ListaEstados { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IdEstado { get; set; }

        #endregion


        #region Métodos
        
        /// <summary> Carregar Estados
        /// </summary>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> CarregarEstados()
        {
            var estado = _repoEstado.Listar().OrderBy(m => m.Nome);

            var lista = new List<SelectListItem>();
            
            foreach (var item in estado)
            {
                var slitem = new SelectListItem()
                {
                    Text = item.Nome,
                    Value = item.Uf,
                };

                lista.Add(slitem);
            }

            return lista;
        }

        #endregion

    }
}