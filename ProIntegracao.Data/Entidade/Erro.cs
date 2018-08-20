using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data.Entidade
{
    public class Erro : Entity
    {
        #region Propriedades
        public virtual Estado Estado { get; set; }
        public virtual string CodigoErro { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Descricao { get; set; }
        public virtual DateTime DtCadastro { get; set; }

        #endregion
    }
}
