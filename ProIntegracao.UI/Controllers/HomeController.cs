using ProIntegracao.Data.Entidade;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ProIntegracao.Model.Repositorio;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>Home Controller
    /// </summary>
    public class HomeController : BaseController
    {

        #region Variáveis

        public static readonly RepositorioMenu _repo = new RepositorioMenu();
        public static readonly RepositorioPerfilPagina _repoPerfilPagina = new RepositorioPerfilPagina();

        #endregion

        #region Actions

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>

        public ActionResult Index(string nome)
        {
            Menu menu = new Menu();

            if (!string.IsNullOrEmpty(nome))
            {
                menu = _repo.Listar().Where(m => m.Nome == nome).FirstOrDefault();
            }

            ViewBag.Title = RetornaNomePagina("HOME");

            return View(RetornaPaginasUnicas(menu));

        }

        /// <summary>Lista Atalho de Icones
        /// </summary>
        /// <returns></returns>
        public ActionResult CheatSheet() {

            return PartialView();
        }

        #endregion

        #region Métodos

        /// <summary>Retorno Paginas Unicas
        /// </summary>
        /// <returns>List Pagina</returns>
        //public List<Pagina> RetornaPaginasUnicas()
        //{
        //    var lista = new List<Pagina>();

        //    //Página HOME
        //    lista.Add(new Pagina()
        //    {
        //        Id = 0
        //        , Nome = "Home"
        //        , Url = "Home"
        //        , Icone = "fa fa-home"
        //        , Menu = new Menu() {
        //            Id = 0
        //            , Nome = "Home"
        //            , Ordem = 1
        //        }
        //    });


        //    try
        //    {
        //        lista.AddRange(Menu);
        //    }
        //    catch (Exception ex)
        //    {
        //        //InserirLog("HOME", "Erro Distinct MENU - Erro :" + ex.Message);
        //    }

        //    return lista.OrderBy(m => m.Menu.Ordem).ToList();
        //}


        /// <summary>Retorno Paginas Unicas
        /// </summary>
        /// <returns>List Pagina</returns>
        public List<Pagina> RetornaPaginasUnicas(Menu menu)
        {
            var lista = new List<Pagina>();

            if (menu.Id == 0)
            {

                var paginasmenus = new List<Menu>();

                if (User.Usuario.Perfil.Admin) { 
                    paginasmenus = _repo.Listar().Where(m => m.MenuPai == null && m.DtExclusao == null).ToList();
                } else {
                        paginasmenus = _repoPerfilPagina
                        .Listar()
                        .Where(m => m.Perfil == User.Usuario.Perfil && m.Pagina.Menu.DtExclusao == null)
                        .Select(m => m.Pagina.Menu)
                        .Distinct()
                        .ToList();
                }

                foreach (var item in paginasmenus)
                {
                    if (!item.Nome.ToUpper().Contains("HOME"))
                    {
                        
                            lista.Add(new Pagina()
                            {
                                Id = item.Ordem,
                                Nome = item.Nome,
                                Url = string.Format("/Home/Index?nome={0}", item.Nome),
                                Icone = "",
                                Menu = null
                            });
                        
                    }
                }
            }
            else {

                if (User.Usuario.Perfil.Admin)
                    lista.AddRange(Menu.Where(m => m.Menu == menu && !m.Menu.Nome.ToUpper().Contains("HOME") && m.DtExclusao == null));
                else
                    lista.AddRange(Menu.Where(m => m.Menu == menu 
                                            && !m.Menu.Nome.ToUpper().Contains("HOME") 
                                            && m.DtExclusao == null
                                            && !m.Menu.Admin));



                lista.Add(new Pagina()
                {
                    Id = 0,
                    Nome = "Voltar",
                    Url = "/Home",
                    Icone = "fa fa-arrow-left",
                    Menu = null,
                    Ordem = 500000
                });
            }
            
            return lista.OrderBy(m => m.Ordem).ToList();
        }



       


        #endregion
    }
}