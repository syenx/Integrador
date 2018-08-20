using NHibernate.Mapping.ByCode;
using ProIntegracao.API.Entidade;
using ProIntegracao.API.Maping;

namespace ProIntegracao.API
{
    public class MatriculaMap : EntityMapping<Matricula>
    {

        public MatriculaMap()
        {
            Table("TbMatricula");
            Id(
                m => m.Id,
                map =>
                {
                    map.Column("IdMatricula");
                    map.Generator(Generators.Identity);
                }
            );
            ManyToOne(p => p.Aluno, map => map.Column("IdAluno"));
            ManyToOne(p => p.Estado, map => map.Column("IdEstado"));
            Property(m => m.QtdAula, map => map.Column("QtdAula"));
            Property(m => m.HoraAula, map => map.Column("HoraAula"));
            Property(m => m.Psa, map => map.Column("PSA"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
            Property(m => m.DtExclusao, map => map.Column("DtExclusao"));
        }
    }
}
