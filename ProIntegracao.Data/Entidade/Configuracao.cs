using System;
namespace ProIntegracao.Data.Entidade
{
    public class Configuracao : Entity
    {
        public virtual string Nome { get; set; }

        public virtual string Descricao { get; set; }

        public virtual string Valor { get; set; }

        public virtual DateTime DtCadastro { get; set; }

        public virtual DateTime? DtExclusao { get; set; }
    }
}
