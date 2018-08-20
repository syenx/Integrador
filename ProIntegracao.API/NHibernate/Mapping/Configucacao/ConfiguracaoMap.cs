using NHibernate.Mapping.ByCode;
using ProIntegracao.API.Entidade;
using ProIntegracao.API.Maping;

namespace ProIntegracao.API
{
    public class ConfiguracaoMap: EntityMapping<Configuracao>
    {
        public ConfiguracaoMap()
        {
            Table("TBConfiguracao");
            Id(
                m => m.Id,
                map =>
                {
                    map.Column("IdConfiguracao");
                    map.Generator(Generators.Identity);
                }
            );
            Property(m => m.Nome, map => map.Column("Nome"));
            Property(m => m.Descricao, map => map.Column("Descricao"));
            Property(m => m.Valor, map => map.Column("Valor"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
            Property(p => p.DtExclusao, map => map.Column("DtExclusao"));
        }
    }
}
