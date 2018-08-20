using NHibernate.Mapping.ByCode;
using ProIntegracao.API.Entidade;
using ProIntegracao.API.Maping;

namespace ProIntegracao.API
{
    public class AulaMap : EntityMapping<Aula>
    {
        public AulaMap()
        {
            Table("TbAula");
            Id(
                m => m.Id,
                map =>
                {
                    map.Column("IdAula");
                    map.Generator(Generators.Identity);
                }
            );
            ManyToOne(m => m.Matricula, map => map.Column("IdMatricula"));
            ManyToOne(m => m.StatusSituacaoAula, map => map.Column("IdStatusSituacaoAula"));
            Property(m => m.CodigoCfc, map => map.Column("CodigoCfc"));
            Property(m => m.IdentificadorAula, map => map.Column("IdentificadorAula"));
            Property(m => m.CpfInstrutor, map => map.Column("CpfInstrutor"));
            Property(p => p.DataInicioAula, map => map.Column("DataInicioAula"));
            Property(p => p.DataFimAula, map => map.Column("DataFimAula"));
            Property(m => m.TokenInicioAula, map => map.Column("TokenInicioAula"));
            Property(m => m.TokenFimAula, map => map.Column("TokenFimAula"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
            Property(p => p.DtExclusao, map => map.Column("DtExclusao"));
        }
    }
}
