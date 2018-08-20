using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProIntegracao.UI.ViewModel
{
    /// <summary>
    /// LoginViewModel
    /// </summary>
    public class LoginViewModel 
    {
        /// <summary>
        /// Login
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        public Usuario usuario { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// RememberME
        /// </summary>
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }


    }
}