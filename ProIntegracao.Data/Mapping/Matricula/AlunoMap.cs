using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;


namespace ProIntegracao.Data.Mapping.Matricula
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
            Property(m => m.DtNascimento, map => map.Column("DtNascimento"));
            ManyToOne(p => p.Sexo, map => map.Column("IdSexo"));

        }
    }
}
