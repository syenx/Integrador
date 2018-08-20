using NHibernate.Mapping.ByCode;
using ProIntegracao.Data.EntidadeProSimulador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data.Mapping.Perfil
{
    public class RegiaoMap : EntityMapping<Regiao>
    {
        public RegiaoMap()
        {
            Table("TB_REGIAO");
            Id(
                m => m.Id, map =>
                {
                    map.Column("ID_REGIAO");
                    map.Generator(Generators.Identity);
                }
            );

            Property(m => m.Descricao, map => map.Column("DESCRICAO"));
            
        }
    }
}
