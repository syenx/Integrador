using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// Menu View Model
    /// </summary>
    public class MenuViewModel : BaseViewModel
    {
        #region Variáveis

        private static readonly RepositorioMenu _repo = new RepositorioMenu();
        private static readonly RepositorioPagina _repoPagina = new RepositorioPagina();
        private static readonly RepositorioPerfilPagina _repoPerfilPagina = new RepositorioPerfilPagina();
        #endregion

        #region Constrtutor

        /// <summary>
        /// Menu View Model
        /// </summary>
        public MenuViewModel()
        {
            Principal = HttpContext.Current.User as Models.CustomPrincipal;
            Menus = CarregarMenus();
            MenuHtml = RetornaMenuHtml();
            MenuHtmlAdm = RetornaMenuHtmlAdm();
            ListaMenuPai = CarregaMenuPai();
        }

        /// <summary>
        /// Construtor para Edição
        /// </summary>
        /// <param name="idMenu">IdMenu</param>
        public MenuViewModel(int idMenu)
        {
            ListaMenuPai = CarregaMenuPai(idMenu);
            var menu = _repo.Obter<Menu>(idMenu);

            Id          = menu.Id;
            Nome        = menu.Nome;
            Url         = menu.Url;
            Ordem       = menu.Ordem;
            DtExclusao  = menu.DtExclusao;
            DtCadastro  = menu.DtCadastro;

            if (menu.MenuPai != null)
                IdMenuPai = menu.MenuPai.Id;
         
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Paginas
        /// </summary>
        public List<Menu> Menus { get; set; }

        /// <summary>
        /// Menu Html
        /// </summary>
        public string MenuHtml { get; set; }

        public string MenuHtmlAdm { get; set; }

        /// <summary>
        /// Custom Principal
        /// </summary>
        public Models.CustomPrincipal Principal { get; set; }

        /// <summary>
        /// Lista Menu Pai - Principal
        /// </summary>
        public List<SelectListItem> ListaMenuPai { get; set; }

        /// <summary>
        /// Menu Html
        /// </summary>

        [DisplayName("Menu Pai :")]
        public int IdMenuPai { get; set; }

        /// <summary>
        /// Ordem
        /// </summary>
        [DisplayName ("Ordem :")]
        public int Ordem { get; set; }

        /// <summary>
        /// Menu Nestead representa o retorno do plugin NESTEAD
        /// Facilita assim o entendimento e alteração do mesmo
        /// Verifique o arquivo  menu.js para entender aplicação
        /// </summary>
        public List<MenuNestead> MenuNestead { get; set; }


        /// <summary>
        /// Nome
        /// </summary>
        [DisplayName ("Nome :")]
        public string Nome { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        [DisplayName("Url :")]
        public string Url { get; set; }


        #endregion

        #region Métodos

        /// <summary> Retorna Menus
        /// </summary>
        /// <returns></returns>
        public List<Menu> CarregarMenus()
        {
            var lista = new List<Menu>();
            try
            {
                lista = _repo.Listar().ToList();
            }
            catch (Exception ex)
            {
                
            }

            return lista.ToList();
        }

        /// <summary>
        /// Carregar Menu Pai
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> CarregaMenuPai(int idMenu = 0) {

            var model = _repo.Listar().Where(m => m.MenuPai == null).OrderBy(m=>m.Ordem).ToList();

            if (idMenu > 0)
                model = model.Where(m => m.Id != idMenu).ToList();

            var lista = new List<SelectListItem>();
            
            lista.Add(new SelectListItem() {
                Text = "Menu Principal"
                ,Value = "0"
            });
            
            foreach (var item in model)
            {
                var slitem = new SelectListItem()
                {
                    Text = item.Nome,
                    Value = item.Id.ToString(),
                };
                lista.Add(slitem);
            }
            return lista;

        }

        /// <summary> Retorna o Menu Dinâmico para a Aplicação
        /// </summary>
        /// <returns></returns>
        public string RetornaMenuHtml()
        {

            if (Principal == null) return string.Empty;

            var isAdmin = Principal.Usuario.Perfil.Admin;
            var menuPai = new List<Menu>(); 
            var menu = new StringBuilder();

            if (!isAdmin)
            {
                var home = _repo.Listar().Where(m => m.Nome.ToUpper().Equals("HOME")).FirstOrDefault();
                menuPai.Add(home);

                var listaMenus = new List<Menu>();
                listaMenus = _repoPerfilPagina
                    .Listar()
                    .Where(m => m.Perfil == Principal.Usuario.Perfil && m.Pagina.Menu.DtExclusao == null)
                    .Select(m => m.Pagina.Menu)
                    .Distinct()
                    .ToList();

                menuPai.AddRange(listaMenus.Where(m => m.DtExclusao == null).OrderBy(m => m.Ordem).ToList());

            }
            else
            {
                menuPai = _repo.Listar().Where(m => m.MenuPai == null && m.DtExclusao == null).OrderBy(m => m.Ordem).ToList();
            }
            
            // Inicio do MENU
            menu.AppendLine("<ul class='nav navbar-nav'>");

            foreach (var pai in menuPai)
            {
                var count = pai.MenusFilhos.Count();
                count += _repoPagina.Listar().Where(m => m.Menu == pai && m.DtExclusao == null).Distinct().OrderBy(m => m.Ordem).Count();

                var classe = (count > 0) ? "dropdown" : "";
                menu.AppendLine("   <li class='" + classe + "'>");

                if (count > 0)
                    menu.AppendFormat("       <a href='{0}' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>{1}<span class='caret'></span></a>", pai.Url, pai.Nome);
                else
                    menu.AppendFormat("       <a href='{0}' class='dropdown-toggle' role='button' aria-haspopup='true' aria-expanded='false'>{1}</a>", pai.Url, pai.Nome);

                if (pai.Nome.ToUpper() != "HOME") { 
                    RetornaSubMenu(pai, menu);
                }

                menu.AppendLine("   </li>");
            }

            menu.AppendLine("</ul>");
            
            return menu.ToString();
        }
        
        /// <summary>
        /// Retorna Menu Html ADM - Superior
        /// </summary>
        /// <returns></returns>
        public string RetornaMenuHtmlAdm()
        {
            if (Principal == null) return string.Empty;
            var isAdmin = Principal.Usuario.Perfil.Admin;
            var menu = new StringBuilder();
            

            // Menu a direita
            menu.AppendLine("<ul class='nav navbar-nav navbar-right menuAdm'>");
            menu.AppendLine("   <li>");
            menu.AppendFormat("     <a href='#' class='text-info text-capitalize'><strong> Bem-vindo {0} </strong></a>", Principal.Usuario.Login);
            menu.AppendLine("   </li>");

            // Caso seja ADMIN, acrescentar o menu de configuração
            if (isAdmin)
            {
                menu.AppendLine("   <li>");
                menu.AppendLine("       <a href='/Configuracao'>Configurações</a>");
                menu.AppendLine("   </li>");
            }



            menu.AppendLine("   <li class='dropdown'>");
            menu.AppendLine("       <a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'><span class='caret'></span> Minha Conta</a>");
            menu.AppendLine("           <ul class='dropdown-menu'>");
            menu.AppendLine("               <li><a href='/Usuario/AlterarSenha'>Alterar Senha</a></li>");
            menu.AppendLine("           </ul>");
            menu.AppendLine("   </li>");
            
            menu.AppendLine("   <li><a href='/Login/SignOut'>Sair</a></li>");
            menu.AppendLine("</ul>");

            return menu.ToString();
        }
        
        /// <summary>Retorna Sub Menu Recursivo
        /// </summary>
        /// <param name="pai">Item Menu PAI</param>
        /// <param name="menu">String Builder retorno</param>
        /// <returns></returns>
        public StringBuilder RetornaSubMenu(Menu pai, StringBuilder menu)
        {
            var paginas = new List<Pagina>();

            if (Principal.Usuario.Perfil.Admin)
            {
                paginas = _repoPagina
                    .Listar()
                    .Where(m => m.Menu == pai && m.DtExclusao == null)
                    .Distinct()
                    .OrderBy(m => m.Ordem)
                    .ToList();
            }
            else
            {
                paginas = _repoPerfilPagina
                   .Listar()
                   .Where(m => m.Pagina.Menu == pai
                               && m.Perfil == Principal.Usuario.Perfil
                               && m.Pagina.DtExclusao == null)
                   .Select(m => m.Pagina)
                   .Distinct()
                   .OrderBy(m => m.Ordem)
                   .ToList();
            }

            
            menu.AppendLine("       <ul class='dropdown-menu'>");

            foreach (var item in paginas)
            {
                menu.AppendFormat("           <li><a href='{0}'>{1}</a></li>", item.Url, item.Nome);
            }
            
            foreach (var item in pai.MenusFilhos)
            {
                if (item.DtExclusao == null)
                {
                    menu.AppendFormat("           <li><a href='{0}'>{1}</a></li>", item.Url, item.Nome);
                    
                    // Caso o Menu seja PAI 
                    if (item.MenusFilhos.Count() > 0) RetornaSubMenu(item, menu);
                }
            }

            menu.AppendLine("       </ul>");

            return menu;
        }
        
        /// <summary>
        /// Retorna Menu para Edição
        /// </summary>
        /// <returns></returns>
        public string RetornaMenuList()
        {

            if (Principal == null) return string.Empty;

            var isAdmin = Principal.Usuario.Perfil.Admin;
            var menuPai = _repo.Listar().Where(m => m.DtExclusao == null && m.MenuPai == null).OrderBy(m => m.Ordem).ToList();
            var menu = new StringBuilder();

            if (!isAdmin)
            {
                menuPai = menuPai.Where(m => !m.Admin && m.DtExclusao == null).ToList();
            }

            // Inicio do MENU
            menu.AppendLine("<div class='dd' id='nestable'>");
            menu.AppendLine("   <ol class='dd-list'>");

            foreach (var pai in menuPai)
            {
                var count = pai.MenusFilhos.Count();
                var classe = "dd-item";

                var countPaginas = _repoPagina.Listar()
                    .Where(m => m.Menu == pai && m.DtExclusao == null)
                    .OrderBy(m => m.Ordem)
                    .ToList()
                    .Count();


                menu.AppendLine("       <li class='" + classe + "' data-id='"+ pai.Id+ "' data-tipo='menu'>");

                // Verifica se é um MENU DROPDOWN ou NÂO
                if (count > 0)
                {
                    menu.AppendFormat("     <div class='dd-handle menu'>{0}<a href='ui-nestable-list.html#' class=''></a>", pai.Nome);
                    menu.AppendLine();
                    menu.AppendFormat("       <a href='ui-nestable-list.html' class='icon fa fa-pencil' data-id='{0}'></a>", pai.Id);

                    if (countPaginas == 0)
                        menu.AppendFormat("       <a href='ui-nestable-list.html' class='icon fa fa-trash' data-id='{0}'></a>", pai.Id);

                    menu.AppendLine(" </div>");
                }
                else {
                    menu.AppendFormat("     <div class='dd-handle menu'>{1}<a href='{0}' class='ui-nestable-list.html#' class=''></a>", pai.Url, pai.Nome);
                    menu.AppendLine();
                    menu.AppendFormat("       <a href='ui-nestable-list.html' class='icon fa fa-pencil' data-id='{0}'></a>", pai.Id);

                    if (countPaginas == 0)
                        menu.AppendFormat("       <a href='ui-nestable-list.html' class='icon fa fa-trash' data-id='{0}'></a>", pai.Id);

                    menu.AppendLine(" </div>");

                }


                menu.AppendFormat(RetornaPaginas(pai));

                if (count > 0)
                {
                    RetornaSubMenuLista(pai, menu);
                }

                menu.AppendLine("   </li>");
            }

            menu.AppendLine("</ol>");
            menu.AppendFormat("</div>");

            return menu.ToString();


        }
        
        /// <summary>
        /// Retorna Sub Menu LIsta
        /// </summary>
        /// <param name="pai"></param>
        /// <param name="menu"></param>
        /// <returns></returns>
        public StringBuilder RetornaSubMenuLista(Menu pai, StringBuilder menu)
        {
            
            menu.AppendLine("       <ol class='dd-list'>");
            
            foreach (var item in pai.MenusFilhos)
            {
                if (item.DtExclusao == null)
                {
                    menu.AppendFormat("         <li class='dd-item' data-id='{0}' data-tipo='menu'>", item.Id);
                    menu.AppendFormat("         <div class='dd-handle menu'>{0}", item.Nome);
                    menu.AppendLine();
                    menu.AppendFormat("         <a href='ui-nestable-list.html' class='icon fa fa-pencil' data-id='{0}'></a>", item.Id);
                    menu.AppendFormat("         <a href='ui-nestable-list.html' class='icon fa fa-trash'  data-id='{0}'></a>", item.Id);
                    menu.AppendLine("</div>");

                    // Caso o Menu seja 
                    if (item.MenusFilhos.Count() > 0) RetornaSubMenuLista(item, menu);
                }
            }
            
            menu.AppendLine("       </ol>");

            return menu;
        }

        /// <summary>
        /// Retorna Páginas lista de Menu
        /// </summary>
        /// <param name="menu">Menu Item</param>
        /// <returns></returns>
        public string RetornaPaginas(Menu menu)
        {
            var lista = _repoPagina.Listar()
                .Where(m => m.Menu == menu && m.DtExclusao == null)
                .OrderBy(m => m.Ordem)
                .ToList();

            var builder = new StringBuilder();
            
            if (lista.Count() > 0)
            {

                builder.AppendLine("       <ol class='dd-list'>");

                foreach (var item in lista)
                {
                    builder.AppendFormat("         <li class='dd-item pagina' data-id='{0}' data-tipo='pagina'>", item.Id);
                    builder.AppendLine();
                    builder.AppendFormat("         <div class='dd-handle dd-nodrag pagina'>{0}</div>", item.Nome);
                }
                
                builder.AppendLine("       </ol>");
            }

            return builder.ToString();
        }
        
        /// <summary>
        /// Parse View Model
        /// </summary>
        /// <param name="model">Menu View Model</param>
        /// <returns></returns>
        public MenuNestead ParseViewModel(MenuViewModel model)
        {
            var menus = new List<Menu>();
            var paginas = new List<Pagina>();
            
            if (model.MenuNestead != null)
            {
                var array = model.MenuNestead;

                foreach (var item in array)
                {
                    var menu = new Menu();

                    if (item.tipo.ToUpper() == "MENU")
                    {
                        menu = _repo.Obter<Menu>(item.id);
                        menu.MenuPai = null;
                        menus.Add(menu);
                    }
                    else
                    {
                        var pagina = new Pagina();
                        pagina = _repoPagina.Obter<Pagina>(item.id);
                        paginas.Add(pagina);
                    }

                    
                    if (item.children != null)
                    {
                        foreach (var it in item.children)
                        {
                            if (it.tipo.ToUpper() == "MENU")
                            {
                                var menufilho = RetornaMenuAjuste(it, menu.Id);
                                menus.AddRange(menufilho);
                            }
                            else
                            {
                                var paginaFilho = new Pagina();
                                paginaFilho = _repoPagina.Obter<Pagina>(it.id);
                                paginaFilho.Menu = menu;
                                paginas.Add(paginaFilho);
                            }
                        }
                    }
                }
            }

            var menuNestead = new MenuNestead()
            {
                Menus = menus
                , Paginas = paginas
            };


            return menuNestead;
        }

        /// <summary>
        /// Parse View Model - CREATE
        /// </summary>
        /// <param name="model">Menu View Model</param>
        /// <returns>Menu</returns>
        public Menu ParseViewModelCreate(MenuViewModel model)
        {

            var menu = new Menu();
            if (model.Id > 0) menu = _repo.Obter<Menu>(model.Id);

            menu.Nome = model.Nome;
            menu.Url = model.Url;
            
            if (model.IdMenuPai == 0)
                menu.MenuPai = null;
            else
                menu.MenuPai = _repo.Obter<Menu>(model.IdMenuPai);

            var ordem = model.Ordem;

            menu.Ordem = (menu.Id > 0)? ordem : (_repo.Listar().OrderByDescending(m => m.Ordem).First().Ordem + 1);
            
            menu.DtCadastro = model.DtCadastro;
            menu.DtExclusao = model.DtExclusao;

            return menu;
            
        }

        /// <summary>
        /// Retorna Menu de AJustes
        /// </summary>
        /// <param name="menunestead">Menu Hierarquia</param>
        /// <param name="idPai">id Menu PAI</param>
        /// <returns></returns>
        public List<Menu> RetornaMenuAjuste(MenuNestead menunestead, int idPai)
        {
            var lista = new List<Menu>();

            var menu = _repo.Obter<Menu>(menunestead.id);

            if (menu.MenuPai == null && idPai > 0) menu.MenuPai = new Menu();

            menu.MenuPai = _repo.Obter<Menu>(idPai);

            lista.Add(menu);

            if (menunestead.children != null)
            {
                foreach (var item in menunestead.children)
                {
                    lista.AddRange(RetornaMenuAjuste(item, menu.Id));
                }
            }
            
            return lista;
        }
        
        #endregion
    }
}