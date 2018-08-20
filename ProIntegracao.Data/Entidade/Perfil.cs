using ProIntegracao.Data;
using System;

namespace Prointegracao.Data.Entidade
{
    public class Perfil : Entity
    {
        public virtual string Nome { get; set; }

        public virtual bool Admin { get; set; }

        public virtual DateTime DtCadastro { get; set; }

        public virtual DateTime? DtExclusao { get; set; }
    }
}