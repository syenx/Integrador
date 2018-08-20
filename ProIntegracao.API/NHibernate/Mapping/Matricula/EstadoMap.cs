using NHibernate.Mapping.ByCode;
using ProIntegracao.API.Entidade;
using ProIntegracao.API.Maping;

namespace ProIntegracao.API
{
    public class EstadoMap : EntityMapping<Estado>
    {

        public EstadoMap()
        {
            Table("TbEstado");
            Id(
                m => m.Id, map =>
                {
                    map.Column("IdEstado");
                    map.Generator(Generators.Identity);
                }
            );
            Property(m => m. Uf, map => map.Column("Uf"));
            Property(m => m.Nome, map => map.Column("Nome"));
        }
    }
}

