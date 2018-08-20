using NHibernate.Mapping.ByCode;
using ProIntegracao.API.Entidade;
using ProIntegracao.API.Maping;

namespace ProIntegracao.API
{
    public class AlunoMap : EntityMapping<Aluno>
    {

        public AlunoMap()
        {
            Table("TbAluno");
            Id(
                m => m.Id,
                map =>
                {
                    map.Column("IdAluno");
                    map.Generator(Generators.Identity);
                }
            );
            Property(m => m.Nome, map => map.Column("Nome"));
            Property(m => m.CpfAluno, map => map.Column("CpfAluno"));
            Property(m => m.Renach, map => map.Column("Renach"));
            Property(m => m.DtNascimento, map => map.Column("DtNascimento"));
            ManyToOne(p => p.Sexo, map => map.Column("IdSexo"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
            Property(p => p.DtExclusao, map => map.Column("DtExclusao"));
        }
    }
}
