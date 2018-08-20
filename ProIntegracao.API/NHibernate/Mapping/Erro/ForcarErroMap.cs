using NHibernate.Mapping.ByCode;
using ProIntegracao.API.Entidade;
using ProIntegracao.API.Maping;

namespace ProIntegracao.API
{
    public class ForcarErroMap : EntityMapping<ForcarErro>
    {
        public ForcarErroMap()
        {
            Table("TbForcarErro");
            Id(
                m => m.Id,
                map =>
                {
                    map.Column("IdForcarErro");
                    map.Generator(Generators.Identity);
                }
           );

            ManyToOne(p => p.Aluno, map => map.Column("IdAluno"));
            ManyToOne(p => p.Erro, map => map.Column("IdTipoErro"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
            
        }
    }
}
