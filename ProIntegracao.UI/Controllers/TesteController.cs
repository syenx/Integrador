using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using ProIntegracao.Model.Repositorio;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>Teste
    /// </summary>
    public class TesteController : BaseController
    {
        public static readonly RepositorioMenu _repo = new RepositorioMenu();


        #region Actions

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        //public ActionResult Index()
        //{
        //    ViewBag.Title = RetornaNomePagina("HOME");
        //    return View(RetornaPaginasUnicas());
        // }


        public ActionResult Index()
        {

            return View("~/Views/Teste/Index2.cshtml");

            //Menu menu = new Menu();

            //if (!string.IsNullOrEmpty(nome))
            //{
            //    menu = _repo.Listar().Where(m => m.Nome == nome).FirstOrDefault();
            //}

            //return View(RetornaPaginasUnicas(menu));

        }

        /// <summary>Lista Atalho de Icones
        /// </summary>
        /// <returns></returns>
        public ActionResult CheatSheet()
        {
            return PartialView();
        }



        public ActionResult ServerSide()
        {
            return View();
        }


        #endregion

        #region Métodos

        /// <summary>Retorno Paginas Unicas
        /// </summary>
        /// <returns>List Pagina</returns>
        public List<Pagina> RetornaPaginasUnicas(Menu menu)
        {
            var lista = new List<Pagina>();

           

            if (menu.Id == 0)
            {
                var paginasmenus = _repo.Listar().Where(m => m.MenuPai == null && m.DtExclusao == null);
                foreach (var item in paginasmenus)
                {
                    lista.Add(new Pagina()
                    {
                        Id = item.Ordem,
                        Nome = item.Nome,
                        Url = string.Format("/Teste/Index?nome={0}",item.Nome),

                        Icone = (item.Nome.ToUpper() == "HOME")
                                ? "fa fa-home"
                                : "",

                        Menu = null
                    });
                }
                
            }
            else {

                lista.Add(new Pagina()
                {
                    Id = 0 ,Nome = "Home",Url = "/Home",Icone = "fa fa-home",Menu = new Menu(){Id = 0, Nome = "Home",Ordem = 1}
                });

                lista.AddRange(Menu.Where(m => m.Menu == menu));
            }

            lista.Add(new Pagina()
            {
                Id = 0,
                Nome = "Voltar",
                Url = "/Home",
                Icone = "fa fa-arrow-left",
                Menu = null,
                Ordem = 500000
            });
            
            return lista.OrderBy(m => m.Ordem).ToList();
        }

        #endregion
    }
}