using System;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class HistoricoAula
    {
        public string IdAula { get; set; }

        public string IdAgenda { get; set; }

        public string IdSimulador { get; set; }

        public DateTime HorarioInicio { get; set; }

        public DateTime? HorarioFim { get; set; }

        public string Curso { get; set; }

        public string SessionId { get; set; }

        public int IdStatus { get; set; }

        public string Status { get; set; }

        public string ModelodeAula { get; set; }


        public string Info { get; set; }

        public int IdModelo { get; set; }

        public string Estado { get; set; }

        public bool PodeReenviar { get; set; }

        public bool PodeCancelar { get; set; }

        public bool PodeConsultar { get; set; }

        public bool PodeAtualizar { get; set; }

        public bool ConsultarAulaDetran { get; set; }


        public string CPF { get; set; }

        public long IdentificadorAula { get; set; }

        public string IdMatricula { get; set; }


    }
}
