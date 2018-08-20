using System;
namespace ProIntegracao.API.Entidade
{
    public class Matricula : Entity
    {
        public virtual string erroDesc { get; set; }
        public virtual int erroCod { get; set; }

        public virtual Aluno Aluno { get; set; }

        public virtual int QtdAula { get; set; }

        public virtual int CodigoCfc { get; set; }

        public virtual Estado Estado { get; set; }

        public virtual string HoraAula { get; set; }

        public virtual string Psa { get; set; }

        public virtual DateTime DtCadastro { get; set; }

        public virtual DateTime? DtExclusao { get; set; }

    }
}

