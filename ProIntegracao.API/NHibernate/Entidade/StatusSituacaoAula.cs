using System;
namespace ProIntegracao.API.Entidade
{
    public class StatusSituacaoAula : Entity
    {
        public virtual Estado Estado { get; set; }

        public virtual string Nome { get; set; }

        public virtual int Identificador { get; set; }

        public virtual DateTime DtCadastro { get; set; }

        public virtual DateTime? DtExclusao { get; set; }
    }
}
