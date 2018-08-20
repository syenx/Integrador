using System;

namespace ProIntegracao.API.Entidade
{
    public class ForcarErro : Entity
    {
        public virtual Aluno Aluno { get; set; }

        public virtual Erro Erro { get; set; }

        public virtual DateTime DtCadastro { get; set; }


    }
}
