using NHibernate.Mapping.ByCode;
using ProIntegracao.API.Maping;

namespace ProIntegracao.API
{
    public class TipoErroMap : EntityMapping<Entidade.Erro>
    {
        public TipoErroMap()
        {
            Table("TbErro");
            Id(
                m => m.Id,
                map =>
                {
                    map.Column("IdErro");
                    map.Generator(Generators.Identity);
                }
            );
            Property(m => m.Nome, map => map.Column("Nome"));
            Property(m => m.Descricao, map => map.Column("Descricao"));
            ManyToOne(p => p.Estado, map => map.Column("IdEstado"));
            Property(m => m.CodigoErro, map => map.Column("CodigoErro"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
        }
    }
}
