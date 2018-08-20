using System;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class Agenda
    {

        public string IdMatricula { get; set; }

        public string IdAgenda { get; set; }

        public string Sequencia { get; set; }

        public DateTime HorarioAula { get; set; }

        public string PSA { get; set; }

        public string SMC { get; set; }

        public string CPFInstrutor { get; set; }

        public int IdInstrutor { get; set; }

        public int IdAluno { get; set; }

        public string NomeInstrutor { get; set; }

        public string ModeloAula { get; set; }

        public string Status { get; set; }
    }
}
