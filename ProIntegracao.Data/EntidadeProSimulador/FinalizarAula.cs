using System.Collections.Generic;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class FinalizarAula
    {

        public string ID_MODELO { get; set; }

        public string ID_AULA { get; set; }
        
        public string NOME { get; set; }
        
        public string ESTADO { get; set; }

        public string ID_CFC { get; set; }

        public string NOME_FANTASIA { get; set;}

        public string ID_MATRICULA { get; set; }

        public string MODELO { get; set; }
        
        public string CPF { get; set; }


        public List<EventosFinalizacao> Eventos { get; set; }
    }
}
