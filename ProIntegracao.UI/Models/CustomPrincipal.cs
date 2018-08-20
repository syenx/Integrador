using ProIntegracao.Data;
using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace ProIntegracao.UI.Models
{
    /// <summary>
    /// Custom Principal
    /// </summary>
    public class CustomPrincipal : ICustomPrincipal
    {

        #region Variáveis

        private static readonly RepositorioPerfilPagina _repoPerfilPagina = new RepositorioPerfilPagina();
        private static readonly RepositorioUsuario _repoUsuario = new RepositorioUsuario();


        #endregion


        #region Construtor

        /// <summary>
        /// CustomPrincipal
        /// </summary>
        /// <param name="login"></param>
        public CustomPrincipal(string login)
        {
            Identity = new GenericIdentity(login);

            try
            {
                Usuario = _repoUsuario.Listar().Where(m => m.Login == login).FirstOrDefault();
                ListaPerfilPagina = _repoPerfilPagina.ListarPerfilPaginaPorUsuario(Usuario);
                Autenticado = true;
            }
            catch (Exception)
            {
                Autenticado = false;
            }
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Identity
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// IsInRole
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public bool IsInRole(string role) { return false; }

        /// <summary>
        /// Usuario
        /// </summary>
        public Usuario Usuario { get; set; }

        /// <summary>
        /// ListaPerfilPagina
        /// </summary>
        public List<PerfilPagina> ListaPerfilPagina { get; set; }

        /// <summary>
        /// Autenticado
        /// </summary>
        public bool Autenticado { get; set; }

        #endregion
    }
}