using NHibernate.Mapping.ByCode.Conformist;

namespace ProIntegracao.Data
{
    public class EntityMapping<T> : ClassMapping<T> where T : Entity
    {
        public EntityMapping()
        {
            Id(u => u.Id);
        }
    }
}
