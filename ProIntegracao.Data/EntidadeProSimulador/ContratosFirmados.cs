
using System;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class ContratosFirmados
    {
        #region Propriedades

        public DateTime Data { get; set; }
        
        public string Estado { get; set; }

        public int NrContratos { get; set; }

        public int NrSimuladoresContratados { get; set; }

        #endregion

    }
}
