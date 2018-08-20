using System;

namespace ProIntegracao.Data.Entidade
{
    public class StatusSituacaoAula : Entity
    {

        #region Propriedades
        
        public virtual Estado Estado { get; set; }

        public virtual string Nome { get; set; }

        public virtual int Identificador { get; set; }

        public virtual DateTime DtCadastro { get; set; }

        public virtual DateTime? DtExclusao { get; set; }

        #endregion

    }
}
