using NHibernate.Mapping.ByCode;

namespace ProIntegracao.Data.Mapping
{
    public class PerfilMap : EntityMapping<Prointegracao.Data.Entidade.Perfil>
    {
        public PerfilMap()
        {
            Table("TbPerfil");
            Id(
                m => m.Id, map =>{map.Column("IdPerfil");
                map.Generator(Generators.Identity);
                }
            );
            Property(m => m.Nome, map => map.Column("Nome"));
            Property(m => m.Admin, map => map.Column("Admin"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
            Property(m => m.DtExclusao, map => map.Column("DtExclusao"));
        }
    }
}