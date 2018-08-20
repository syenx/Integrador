using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;

namespace ProIntegracao.Data
{
    public class PerfilEstadoMap : EntityMapping<PerfilEstado>
    {
        public PerfilEstadoMap()
        {
            Table("TbPerfilEstado");
            Id(
                m => m.Id, map =>
                {
                    map.Column("IdPerfilEstado");
                    map.Generator(Generators.Identity);
                }
            );
            ManyToOne(p => p.Estado, map => map.Column("IdEstado"));
            Property(p => p.IdPerfil, map => map.Column("IdPerfil"));
        }
    }
}
