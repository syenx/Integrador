using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;

namespace ProIntegracao.Data.Mapping.Perfil
{
    public class PerfilPaginaMap : EntityMapping<PerfilPagina>
    {
        public PerfilPaginaMap()
        {
            Table("TbPerfilPagina");
            Id(
                m => m.Id, map =>
                {
                    map.Column("IdPerfilPagina");
                    map.Generator(Generators.Identity);
                }
            );
            ManyToOne(p => p.Pagina, map => map.Column("IdPagina"));
            ManyToOne(p => p.Perfil, map => map.Column("IdPerfil"));
            Property(m => m.Inserir, map => map.Column("Inserir"));
            Property(m => m.Atualizar, map => map.Column("Atualizar"));
            Property(m => m.Excluir, map => map.Column("Excluir"));
            Property(m => m.Consultar, map => map.Column("Consultar"));
        
        }
    }
}

