using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;

namespace ProIntegracao.Data.Mapping
{
    public class MenuMap : EntityMapping<Menu>
    {

        public MenuMap()
        {
            Table("TbMenu");

            Id(
                m => m.Id, map =>
                {
                    map.Column("IdMenu");
                    map.Generator(Generators.Identity);
                }
            );

            Property(m => m.Nome, map => map.Column("Nome"));
            Property(m => m.Ordem, map => map.Column("Ordem"));
            Property(m => m.Admin, map => map.Column("Admin"));
            Property(m => m.Url, map => map.Column("Url"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
            Property(m => m.DtExclusao, map => map.Column("DtExclusao"));

            ManyToOne(m => m.MenuPai, map => map.Column("IdMenuPai"));

            Bag(p => p.MenusFilhos, map =>
            {
                map.Key(k => k.Column("IdMenuPai"));
                map.Inverse(true);
            }, a => a.OneToMany());

        }
    }
}
