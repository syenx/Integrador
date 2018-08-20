using System;

namespace ProIntegracao.Data.Entidade
{
    public class Pagina : Entity
    {
        public virtual Menu Menu { get; set; }
        public virtual string Url { get; set; }
        public virtual string Nome { get; set; }
        public virtual string Icone { get; set; }
        public virtual int Ordem { get; set; }
        public virtual DateTime DtCadastro { get; set; }
        public virtual DateTime? DtExclusao { get; set; }
        public virtual string IdEstado { get; set; }
    }
}