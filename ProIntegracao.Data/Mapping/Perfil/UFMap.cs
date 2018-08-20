using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;
using ProIntegracao.Data.EntidadeProSimulador;

namespace ProIntegracao.Data.Mapping.Perfil
{
    public class UFMap : EntityMapping<UF>
    {
        public UFMap()
        {
            Table("TB_UF");
            Id(
                m => m.Id, map =>
                {
                    map.Column("ID_UF");
                    map.Generator(Generators.Identity);
                }
            );

            Property(m => m.Nome, map => map.Column("NOME"));
            Property(m => m.Sigla, map => map.Column("SIGLA"));
            ManyToOne(m => m.Regiao, map => map.Column("ID_REGIAO"));
            Property(m => m.DiferencaHorario, map => map.Column("DIFERENCA_HORARIO"));
            Property(m => m.DataExigibilidade, map => map.Column("DATA_EXIGIBILIDADE"));
        }
    }
}
