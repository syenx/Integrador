using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;

namespace ProIntegracao.Data.Mapping
{
    public class UsuarioMap : EntityMapping<Usuario>
    {
        public UsuarioMap()
        {
            Table("TbUsuario");
            Id(
                m => m.Id, map =>
                {
                    map.Column("IdUsuario");
                    map.Generator(Generators.Identity);
                }
            );
            ManyToOne(p => p.Perfil, map => map.Column("IdPerfil"));
            Property(m => m.Login, map => map.Column("Login"));
            Property(m => m.Senha, map => map.Column("Senha"));
            Property(m => m.Email, map => map.Column("Email"));
            Property(m => m.Bloqueado, map => map.Column("Bloqueado"));
            Property(m => m.Hash, map => map.Column("Hash"));
            Property(m => m.DtExclusao, map => map.Column("DtExclusao"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
        }
    }
}
