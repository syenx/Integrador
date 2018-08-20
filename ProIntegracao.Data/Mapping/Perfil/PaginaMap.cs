using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;

namespace ProIntegracao.Data.Mapping.Perfil
{
    public class PaginaMap : EntityMapping<Pagina>
    {

        public PaginaMap()
        {
            Table("TbPagina");
            Id(
                m => m.Id, map =>
                {
                    map.Column("IdPagina");
                    map.Generator(Generators.Identity);
                }
            );

            ManyToOne(p => p.Menu, map => map.Column("IdMenu"));
            Property(p => p.IdEstado, map => map.Column("IdEstado"));
            Property(m => m.Url, map => map.Column("Url"));
            Property(m => m.Ordem, map => map.Column("Ordem"));
            Property(m => m.Nome, map => map.Column("Nome"));
            Property(m => m.Icone, map => map.Column("Icone"));
            Property(m => m.DtExclusao, map => map.Column("DtExclusao"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
        }
    }
}

