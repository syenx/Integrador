using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data.Entidade
{
    public class Aluno : Entity
    {
        public virtual string CpfAluno { get; set; }
        public virtual string Nome { get; set; }
        public virtual Sexo Sexo { get; set; }
        public virtual string Renach { get; set; }
        public virtual string DtNascimento{ get; set; }
        public virtual string DtCadastro { get; set; }
        public virtual DateTime? DtExclusao { get; set; }
    }
}
