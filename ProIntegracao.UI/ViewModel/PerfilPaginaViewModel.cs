﻿using System;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// PerfilPaginaViewModel
    /// </summary>
    public class PerfilPaginaViewModel : BaseViewModel
    {

        #region Construtor

        /// <summary>
        /// PerfilPaginaViewModel
        /// </summary>
        public PerfilPaginaViewModel()
        {

        }

        #endregion

        #region Propriedades

        /// <summary>
        /// IdPerfilPagina
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Perfil
        /// </summary>
        public int IdPerfil { get; set; }

        /// <summary>
        /// Pagina
        /// </summary>
        public int IdPagina { get; set; }

        /// <summary>
        /// Inserir
        /// </summary>
        public bool Inserir { get; set; }

        /// <summary>
        /// Atualizar
        /// </summary>
        public  bool Atualizar { get; set; }

        /// <summary>
        /// Excluir
        /// </summary>
        public  bool Excluir { get; set; }

        /// <summary>
        /// Consultar
        /// </summary>
        public  bool Consultar { get; set; }
        
        
      

        #endregion

    }
}