using System;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class UF : Entity
    {
        /// <summary>
        /// Nome
        /// </summary>
        public virtual string Nome { get; set; }

        /// <summary>
        /// Sigla
        /// </summary>
        public virtual string Sigla { get; set; }
        
        /// <summary>
        /// Id Regiao 
        /// </summary>
        public virtual Regiao Regiao { get; set; }

        /// <summary>
        /// Diferenca Horario
        /// </summary>
        public virtual int DiferencaHorario { get; set; }

        /// <summary>
        ///  Data Exigibilidade
        /// </summary>
        public virtual DateTime DataExigibilidade { get; set; }

    }
}
