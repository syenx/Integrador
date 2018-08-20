using Prointegracao.Data.Entidade;

namespace ProIntegracao.Data.Entidade
{
    public class PerfilEstado : Entity
    {
        public virtual int IdPerfil { get; set; }
        public virtual Estado Estado { get; set; }
    }
}
