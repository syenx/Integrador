using ProIntegracao.Data.Entidade;
using System.Security.Principal;

namespace ProIntegracao.UI.Models
{
    interface ICustomPrincipal : IPrincipal
    {
        Usuario Usuario { get; set; }
    }
}