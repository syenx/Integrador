using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.Entidade;

namespace ProIntegracao.Data.Mapping.Matricula
{
    public class StatusSituacaoAulaMap : EntityMapping<StatusSituacaoAula>
    {
        public StatusSituacaoAulaMap()
        {
            Table("TbStatusSituacaoAula");
            Id(
                m => m.Id,
                map =>
                {
                    map.Column("IdStatusSituacaoAula");
                    map.Generator(Generators.Identity);
                }
            );
            Property(m => m.Nome, map => map.Column("Nome"));
            Property(m => m.Identificador, map => map.Column("Identificador"));
            ManyToOne(m => m.Estado, map => map.Column("IdEstado"));
            Property(m => m.DtCadastro, map => map.Column("DtCadastro"));
            Property(p => p.DtExclusao, map => map.Column("DtExclusao"));

        }
    }
}
