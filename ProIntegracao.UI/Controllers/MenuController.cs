using System.Web.Mvc;
using System.Linq;
using System;
using ProIntegracao.UI.ViewModel;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Attribute;
using ProIntegracao.UI.Enum;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Menu Controller
    /// </summary>
    public class MenuController : BaseController
    {
        #region Variáveis

        private static readonly RepositorioMenu _repo = new RepositorioMenu();

        #endregion
        
        #region Actions 

        /// <summary>
        ///  INDEX
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = RetornaNomePagina("/Menu");
            var model = new MenuViewModel();
            return View(model);
        }
        
        /// <summary>
        /// Listar Menu
        /// </summary>
        /// <returns></returns>
        public ActionResult ListarMenu()
        {
            var model = new MenuViewModel();
            return PartialView(model);
        }
        
        /// <summary>
        /// Listar Menu
        /// </summary>
        /// <returns></returns>
        public ActionResult ListarMenuAdm()
        {
            var model = new MenuViewModel();
            return PartialView(model);
        }

        /// <summary>
        /// Listar Menu Edição
        /// </summary>
        /// <returns></returns>
        public ActionResult ListarMenuEdicao()
        {
            var model = new MenuViewModel();
            return Json (new {Resultado = model.RetornaMenuList().ToString() },JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns>View</returns>
        public ActionResult Create()
        {
            var model = new MenuViewModel();
            return PartialView(model);
        }
        
        /// <summary>
        /// Create 
        /// </summary>
        /// <param name="model">Menu View Model</param>
        /// <returns></returns>
        [HttpPost]
        [Permission(Permissoes.Admin)]
        public ActionResult Create(MenuViewModel model)
        {
            var resultado = false;

            try
            {
                model.DtCadastro = DateTime.Now;
                var menu = model.ParseViewModelCreate(model);
                resultado = (_repo.Salvar(menu)>0);
            }
            catch (Exception ex)
            {
                //InserirLog("MENU", ex.Message);
            }

            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Edit - GET
        /// </summary>
        /// <param name="idMenu">Id do Menu</param>
        /// <returns></returns>
        public ActionResult Edit(int idMenu)
        {
            var model = new MenuViewModel(idMenu);
            return PartialView(model);
        }
        
        /// <summary>
        /// Edit 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(MenuViewModel model)
        {
            var resultado = false;

            try
            {
                model.DtCadastro = DateTime.Now;
                var menu = model.ParseViewModelCreate(model);
                resultado = _repo.Atualizar(menu);
            }
            catch (Exception ex)
            {
                
            }

            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }  
        
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Update(MenuViewModel model)
        {
            var resultado = false;
            var menus = model.ParseViewModel(model);
            resultado = _repo.SalvarMenu(menus.Menus, menus.Paginas);
            return Json(new { Resultado = resultado }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>Excluir
        /// </summary>
        /// <param name="id">Excluir</param>
        /// <returns></returns>
        [Permission(Permissoes.Admin)]
        public ActionResult Excluir(int id)
        {
            var result = false;

            try
            {
                var menu = _repo.Listar().Where(m => m.Id == id).SingleOrDefault();
                menu.DtExclusao = DateTime.Now;
                result = (_repo.Atualizar(menu));
            }
            catch (Exception ex)
            {
                //InserirLog("MENU", "Erro ao Excluir registro | Mensagem : " + ex.Message);
            }

            return Json(new { Resultado = result }, JsonRequestBehavior.AllowGet);

        }
        
        #endregion
    }
}