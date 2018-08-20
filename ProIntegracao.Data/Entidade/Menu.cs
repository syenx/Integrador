using System;
using System.Collections.Generic;

namespace ProIntegracao.Data.Entidade
{
    public class Menu : Entity
    {
        public virtual string Nome { get; set; }

        public virtual int Ordem { get; set; }

        public virtual bool Admin { get; set; }

        public virtual string Url { get; set; }
        
        public virtual DateTime DtCadastro { get; set; }

        public virtual DateTime? DtExclusao { get; set; }

        public virtual Menu MenuPai { get; set; }

        public virtual IList<Menu> MenusFilhos { get; set; }

    }
}
