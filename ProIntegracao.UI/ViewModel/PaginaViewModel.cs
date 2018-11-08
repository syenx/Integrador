using ProIntegracao.Data;
using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Linq;
using ProIntegracao.UI.Helper;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// PaginaViewModel
    /// </summary>
    public class PaginaViewModel : BaseViewModel 
    {
        #region Variáveis Públicas

        private static readonly RepositorioPagina _repo = new RepositorioPagina();
        private static readonly RepositorioEstado _repoEstado = new RepositorioEstado();
        private static readonly RepositorioMenu _repoMenu = new RepositorioMenu();

        #endregion

        #region Construtor

        /// <summary>
        /// Pagina View Model
        /// </summary>
        public PaginaViewModel()
        {
            listaMenus = CarregarMenus();
            listaEstados = CarregarEstados();
            Ordem = (_repo.Listar().OrderByDescending(m => m.Ordem).FirstOrDefault().Ordem) + 1;
            DtExclusao = null;
        }
        
        /// <summary>
        /// Pagina View Model
        /// </summary>
        /// <param name="pagina"></param>
        public PaginaViewModel(Pagina pagina)
        {
            Url             = pagina.Url;
            Nome            = pagina.Nome;
            Icone           = pagina.Icone;
            IdMenu          = pagina.Menu.Id;
            Id              = pagina.Id;
            listaMenus      = CarregarMenus(pagina.Menu.Id);
            Ordem           = pagina.Ordem;
            IdEstado        = pagina.IdEstado.Split(',').ToArray();
            listaEstados    = CarregarEstados();
            DtExclusao      = pagina.DtExclusao;
       //     DtCadastro      = pagina.DtCadastro;

            Edicao = true;
        }

        #endregion

        #region Propriedades
        
        /// <summary>
        /// Id Menu
        /// </summary>
        public int IdMenu { get; set; }

        /// <summary>
        /// Id Estado
        /// </summary>
        public string[] IdEstado { get; set; }


        /// <summary>
        /// Ordem
        /// </summary>
        [DisplayName("Ordem : ")]
        public int Ordem { get; set; }
        
        /// <summary>
        /// Url
        /// </summary>
        [DisplayName("Url : ")]
        public string Url { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        [DisplayName("Nome : ")]
        public string Nome { get; set; }

        /// <summary>
        /// Icone
        /// </summary>
        [DisplayName("Ícone :")]
        public string Icone { get; set; }
        
        /// <summary>
        /// lista Menus
        /// </summary>
        public IEnumerable<SelectListItem> listaMenus { get; set; }


        /// <summary>
        /// Lista Estados
        /// </summary>
        public List<SelectListItem> listaEstados { get; set; }

        /// <summary>
        /// Edicao
        /// </summary>
        public bool Edicao { get; set; }

        #endregion

        #region Métodos

        /// <summary> Carregar Menus
        /// </summary>
        /// <param name="id">ID Menu</param>
        /// <returns>ListSelectListItem </returns>
        public List<SelectListItem> CarregarMenus(int id = 0)
        {
            var menu = _repoMenu.Listar().Where(m => m.DtExclusao == null).OrderBy(m => m.Nome).ToList();
            var lista = new List<SelectListItem>();

            foreach (var item in menu)
            {
                if (item.MenuPai != null) {
                    if (item.MenuPai.DtExclusao != null)
                    {
                        continue;
                    }
                }
                        
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

        /// <summary> Carregar Estados
        /// </summary>
        /// <returns>List SelectListItem</returns>
        public List<SelectListItem> CarregarEstados()
        {
            var estado = _repoEstado.Listar().OrderBy(m=> m.Nome);
            
            var lista = new List<SelectListItem>();

            foreach (var item in estado)
            {
                var slitem = new SelectListItem()
                {
                    Text = (item.Nome),
                    Value = item.Uf,
                };

                lista.Add(slitem);
            }
            
            return lista;
        }

        /// <summary> Parse Pagina View Model
        /// </summary>
        /// <param name="model">Pagina View Model</param>
        /// <returns>Pagina</returns>
        public Pagina ParsePaginaViewModel(PaginaViewModel model)
        {
            var pagina = new Pagina();

            if (model.Id > 0) pagina = _repo.Obter<Pagina>(model.Id);

            pagina.Id           = (pagina == null) ? model.Id : pagina.Id;
            pagina.Url          = model.Url;
            pagina.Nome         = model.Nome;
            pagina.Icone        = model.Icone;
            pagina.Ordem        = model.Ordem;
            pagina.Menu         = _repoMenu.Obter<Menu>(model.IdMenu);
            pagina.IdEstado     = String.Join(",",model.IdEstado);
          //  pagina.DtCadastro   = model.DtCadastro;
            pagina.DtExclusao   = model.DtExclusao;
            pagina.Id           = model.Id;
           
            return pagina;
        }
        
        #endregion
    }
}