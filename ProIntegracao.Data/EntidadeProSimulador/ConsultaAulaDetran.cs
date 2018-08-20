using System;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class ConsultaAulaDetran
    {
        #region Retorno  

        public string CNPJCFC { get; set; }
        public string NumeroSerie { get; set; }
        public string RENACH { get; set; }
        public string CPFInstrutor { get; set; }
        public string Data { get; set; }
        
        #endregion

        #region Entrada

        public Int64 identinficadorAula { get; set; }
        public string cpf { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public string MensagemRetorno { get; set; }
        public string ResponsavelPeloCancelamento { get; set; }

        #endregion
    }
}
