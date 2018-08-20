using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.Enum;
using ProIntegracao.UI.Models;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace ProIntegracao.UI.Attribute
{
    /// <summary>
    /// Permission Attribute
    /// </summary>
    public class PermissionAttribute : ActionFilterAttribute
    {
        #region Variárveis

        private static readonly RepositorioPagina           _repo = new RepositorioPagina();
        private static readonly RepositorioPerfilPagina     _repoPerfilPagina = new RepositorioPerfilPagina();

        #endregion

        #region Propriedades

        private readonly Permissoes permissoes;

        private static CustomPrincipal principal { get; set; }

        #endregion

        #region Construtor

        /// <summary>
        /// Permission Attribute
        /// </summary>
        /// <param name="requerido">Permissões</param>
        public PermissionAttribute(Permissoes requerido)
        {
            permissoes = requerido;
        }

        #endregion

        #region Actions

        /// <summary>
        /// On ActionExecuting
        /// </summary>
        /// <param name="filterContext">ActionExecutingContext</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
            var controllerName          = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper();
            principal                   = filterContext.HttpContext.User as CustomPrincipal;

            if (principal.Usuario == null) RedirectToLogin(filterContext);

            //// Se for Admin tem acesso irrestrito
            if (principal.Usuario.Perfil.Admin) return;

            var permissao = false;

            foreach (var item in principal.ListaPerfilPagina)
            {
                switch (permissoes)
                {
                    case Permissoes.Consultar:
                        permissao = (item.Consultar && item.Pagina.Url.Replace(@"/", "").ToUpper() == controllerName);
                        break;
                    case Permissoes.Inserir:
                        permissao = (item.Inserir && item.Pagina.Url.Replace(@"/", "").ToUpper() == controllerName);
                        break;
                    case Permissoes.Atualizar:
                        permissao = (item.Atualizar && item.Pagina.Url.Replace(@"/", "").ToUpper() == controllerName);
                        break;
                    case Permissoes.Excluir:
                        permissao = (item.Excluir && item.Pagina.Url.Replace(@"/", "").ToUpper() == controllerName);
                        break;
                    case Permissoes.Admin:
                        permissao = (true);
                        break;
                }

                if (permissao) break;
                
            }

            if (permissao && permissoes != Permissoes.Admin)
            {
                
                var result = PermiteAcessoEstadoPorPagina(controllerName);
                
                if (!result)
                {
                    RedirectToNaoAutorizado(filterContext);
                }
            }
            else
            {
                if (permissoes != Permissoes.Admin)
                    RedirectToNaoAutorizado(filterContext);
            }

        }

        private static void RedirectToNaoAutorizado(ActionExecutingContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            var loginUrl = url.Content("~/Error/NaoAutorizado");
            filterContext.HttpContext.Response.Redirect(loginUrl, true);
        }

        private static void RedirectToLogin(ActionExecutingContext filterContext)
        {
            var url = new UrlHelper(filterContext.RequestContext);
            var loginUrl = url.Content("~/Login/Index");
            filterContext.HttpContext.Response.Redirect(loginUrl, true);
        }


        /// <summary>
        /// Lista de Estados por Pagina
        /// </summary>
        /// <param name="controllername">Nome do Controller</param>
        /// <returns></returns>
        public bool PermiteAcessoEstadoPorPagina(string controllername)
        {

            var result = false;

            var estadosporPagina = _repo.Listar().Where(m => m.Url.Replace(@"/", "").ToUpper() == controllername).Select(m => m.IdEstado).ToList();
            estadosporPagina = AjusteArrayEstados(estadosporPagina);
            var estadosporPerfilPagina = _repoPerfilPagina.Listar().Where(m => m.Perfil.Id == principal.Usuario.Perfil.Id).Select(m => m.Pagina.IdEstado).Distinct().ToList();
            estadosporPerfilPagina = AjusteArrayEstados(estadosporPerfilPagina);

            //Os Estados Desta Pagina estão Liberados de acordo com os dados de Perfil
            foreach (var item in estadosporPagina.ToArray())
            {
                foreach (var estado in estadosporPerfilPagina)
                {
                    if (item == estado)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Ajuste Array Estados
        /// </summary>
        /// <param name="estados">Lista string de Estados</param>
        /// <returns></returns>
        public List<string> AjusteArrayEstados(List<string> estados)
        {
            var lista = new List<string>();

            foreach (var item in estados)
            {
                var it = item.Split(',');
                if (it.Length > 1)
                {
                    foreach (var i in it)
                    {
                        lista.Add(i);
                    }
                }
                else
                {
                    lista.Add(item);
                }
            }

            lista = lista.Distinct().ToList();

            return lista;
        }

        #endregion
    }
}