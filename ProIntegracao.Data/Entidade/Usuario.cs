using Prointegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data.Entidade
{
    public class Usuario : Entity
    {
        public virtual Perfil Perfil { get; set; }

        public virtual string Login { get; set; }

        public virtual string Senha { get; set; }

        public virtual string Email { get; set; }

        public virtual DateTime? DtExclusao { get; set;}

        public virtual DateTime DtCadastro { get; set; }

        public virtual bool Bloqueado { get; set; }

        public virtual string Hash { get; set; }

    }
}
