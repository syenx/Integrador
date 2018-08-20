using NHibernate.Mapping.ByCode;
using ProIntegracao.API.Entidade;
using ProIntegracao.API.Maping;

namespace ProIntegracao.API.Maping
{
    public class SexoMap : EntityMapping<Sexo>
    {
        public SexoMap()
        {
            Table("Tbsexo");
            Id(
                m => m.Id,
                map =>
                {
                    map.Column("IdSexo");
                    map.Generator(Generators.Identity);
                }
            );
            Property(m => m.Nome, map => map.Column("Nome"));
        }
    }
}
