using System;
using System.Collections.Generic;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class Aluno
    {
        public string CFC { get; set; }

        public string Nome { get; set; }

        public string Renach { get; set; }

        public string Instrutor { get; set; }

        public string CPFInstrutor { get; set; }
        
        public string Modelo { get; set; }

        public string Session_ID { get; set; }

        public string VelocidadeMedia { get; set; }
        
        public string TempoTrajeto { get; set; }

        public DateTime HorarioInicio { get; set; }

        public DateTime? HorarioFinal { get; set; }
        
        public List<Erro> Erros { get; set; }

    }


   
}
