using System;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class AulaRealizada
    {

        #region Propriedades

        public DateTime Data { get; set; }
        
        public string SMC { get; set; }

        public string CNPJ { get; set; }

        public string RazaoSocial { get; set; }
        
        public string UF { get; set; }

        public int AulasRealizadas { get; set; }
        
        #endregion

    }
}
