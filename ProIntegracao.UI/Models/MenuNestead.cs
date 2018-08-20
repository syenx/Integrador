using ProIntegracao.Data.Entidade;
using System.Collections.Generic;

namespace ProIntegracao.UI.Models
{
    /// <summary>
    /// Menu Nestead
    /// </summary>
    public class MenuNestead
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Tipo (Menu ou Pagina)
        /// </summary>
        public string tipo { get; set; }

        /// <summary>
        /// children
        /// </summary>
        public List<MenuNestead> children { get; set; }
        
        /// <summary>
        /// Lista de Menus
        /// </summary>
        public List<Menu> Menus { get; set; }

        /// <summary>
        /// Lista de Páginas
        /// </summary>
        public List<Pagina> Paginas { get; set; }

    }
}