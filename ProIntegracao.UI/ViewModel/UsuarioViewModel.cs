using ProIntegracao.Data.Entidade;
using System.Collections.Generic;
using System.Web.Mvc;
using System;
using ProIntegracao.Model.Repositorio;
using System.Linq;
using Prointegracao.Data.Entidade;
using ProIntegracao.UI.Helper;
using System.ComponentModel;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// UsuarioViewModel
    /// </summary>
    public class UsuarioViewModel : BaseViewModel
    {

        #region Variaveis

        private static readonly RepositorioUsuario _repo = new RepositorioUsuario();
        private static readonly RepositorioPerfil _repoPerfil = new RepositorioPerfil();

        #endregion
        
        #region Construtor

        /// <summary>Construtor Padrão
        /// </summary>
        public UsuarioViewModel()
        {
            //Carrega Lista de Perfis
            listaPerfis = CarregarPerfis();
            Bloqueado = false;
            Edicao = false;
        }

        /// <summary>Usuario View Model
        /// </summary>
        /// <param name="usuario">Entidade Usuario</param>
        public UsuarioViewModel(Usuario usuario)
        {
            Id              = usuario.Id;
            Login           = usuario.Login;
            Email           = usuario.Email;
            IdPerfil        = usuario.Perfil.Id;
            Bloqueado       = usuario.Bloqueado;
            DtCadastro      = usuario.DtCadastro;
            DtExclusao      = usuario.DtExclusao;

            listaPerfis     = CarregarPerfis(usuario.Perfil.Id);
            listaBloqueado  = CarregarBloqueado(usuario.Bloqueado);

            //Habilita os forms de Edição
            Edicao = true;
            
        }

        #endregion

        #region Propriedades

        /// <summary>
        /// Login
        /// </summary>
        [DisplayName("Login :")]
        public string Login { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [DisplayName("Senha :")]
        public string Senha { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [DisplayName("E-mail :")]
        public string Email { get; set; }

        /// <summary>
        /// Bloqueado
        /// </summary>
        [DisplayName("Bloqueado :")]
        public bool Bloqueado { get; set; }

        /// <summary>
        /// Id Perfil
        /// </summary>
        [DisplayName("Perfil :")]
        public int IdPerfil { get; set; }

        /// <summary>
        /// Lista Perfis
        /// </summary>
        public IEnumerable<SelectListItem> listaPerfis { get; set; }

        /// <summary>
        /// Lista Bloqueado
        /// </summary>
        public IEnumerable<SelectListItem> listaBloqueado { get; set; }
        
        /// <summary>
        /// Edicao
        /// </summary>
        public bool Edicao { get; set; }

        #endregion

        #region Métodos

        /// <summary> Carregar uma SelectLIst para Status
        /// </summary>
        /// <param name="id">Id Perfil</param>
        /// <returns>Select List Item</returns>
        public List<SelectListItem> CarregarPerfis(int id = 0)
        {
            var perfil = _repoPerfil.Listar().OrderBy(m => m.Nome).ToList();

            var lista = new List<SelectListItem>();

            foreach (var item in perfil)
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
        
        /// <summary> Carregar uma SelectLIst para Bloqueado
        /// </summary>
        /// <param name="bloqueado">True / False</param>
        /// <returns>SeletListItem</returns>
        public List<SelectListItem> CarregarBloqueado(bool bloqueado = false)
        {

            var lista = new List<SelectListItem>();

            var slitem = new SelectListItem()
            {
                Text = "SIM",
                Value = "true",
                Selected = (bloqueado) ? true : false
            };

            var slitem1 = new SelectListItem()
            {
                Text = "NÃO",
                Value = "false",
                Selected = (!bloqueado) ? true : false
            };

            lista.Add(slitem);
            lista.Add(slitem1);

            return lista;
        }
        
        /// <summary>Parse View Model para Entidade Usuario
        /// </summary>
        /// <param name="model">Usuario View Model</param>
        /// <returns>Usuario</returns>
        public Usuario ParseUsuarioViewModel(UsuarioViewModel model)
        {
            var usuario = new Usuario();

            if (model.Id > 0)
            {
                usuario = _repo.Obter<Usuario>(model.Id);
            }

            usuario.Login           = model.Login;
            usuario.Email           = model.Email;
            usuario.Bloqueado       = model.Bloqueado;
            usuario.Perfil          = _repoPerfil.Obter<Perfil>(model.IdPerfil);
            usuario.DtExclusao      = model.DtExclusao;
            usuario.DtCadastro      = (model.Edicao) ? model.DtCadastro : DateTime.Now;
            usuario.Hash            = Guid.NewGuid().ToString();
            return usuario;
        }
        
        #endregion
    }
}