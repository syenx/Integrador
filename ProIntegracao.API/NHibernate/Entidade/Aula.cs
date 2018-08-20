﻿using System;

namespace ProIntegracao.API.Entidade
{
    public class Aula : Entity
    {
        public virtual int erroCod { get; set; }
        public virtual string erroDescr { get; set; }

        public virtual Matricula Matricula { get; set; }

        public virtual string CpfInstrutor { get; set; }
        public virtual string CodigoCfc { get; set; }

        public virtual string IdentificadorAula { get; set; }

        public virtual DateTime? DataInicioAula { get; set; }

        public virtual DateTime? DataFimAula { get; set; }

        public virtual StatusSituacaoAula StatusSituacaoAula { get; set; }

        public virtual string TokenInicioAula { get; set; }

        public virtual string TokenFimAula { get; set; }

        public virtual DateTime DtCadastro { get; set; }

        public virtual DateTime? DtExclusao { get; set; }

    }
}
