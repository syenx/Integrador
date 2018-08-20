using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;

namespace ProIntegracao.Data
{
    public class SexoMap : EntityMapping<Sexo>
    {
        public SexoMap()
        {
            Table("TbSexo");
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
