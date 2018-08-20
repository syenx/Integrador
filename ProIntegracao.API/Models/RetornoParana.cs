using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProIntegracao.API.Models
{
    public class Agendamento
    {
        public string horario { get; set; }
        //Código do CFC
        public String codCfc { get; set; }
        //Código da Turma
        public String codTurmaCfc { get; set; }
        //Data (dd/mm/aaaa)
        public String dataCurso { get; set; }
        //Código do equipamento
        public String codEquipamentoSimulador { get; set; }
        //Descr modelo do equipamento
        public String modeloSimulador { get; set; }

    }

    public class Ocorrencia
    {
        // Número do processo sem pontos, traços ou caracteres especiais;
        public String numProcesso { get; set; }
        // Código do CFC onde o candidato fará aula;
        public String codCfc { get; set; }
        // Código da turma sem pontos, traços ou caracteres especiais;
        public String codTurmaCfc { get; set; }
        // Data agendada do curso (dd/mm/aaaa);
        public String dataCurso { get; set; }
        // Hora inicial do curso (hh:mm);
        public String horaInicio { get; set; }
        // Código do equipamento simulador;
        public String codEquipamentoSimulador { get; set; }
        public String operacao { get; set; }
    }
    public class RetornoWS
    {
        // Número do processo sem pontos, traços ou caracteres especiais;
        public int codStatus { get; set; }
        // Código do CFC onde o candidato fará aula;
        public String mensagem { get; set; }
    }
}