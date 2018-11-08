using System.ComponentModel;
namespace ProIntegracao.UI.Enum
{
    /// <summary>
    /// 
    /// </summary>
    public enum StatusAula
    {
        /// <summary>
        /// 
        /// </summary>
        [Description("INICIADA")]
        INICIADA = 1,
        /// <summary>
        /// 
        /// </summary>
        [Description("EM ANDAMENTO")]
        EM_ANDAMENTO = 2,
        /// <summary>
        /// 
        /// </summary>
        [Description("REALIZADA")]
        REALIZADA = 3,
        /// <summary>
        /// 
        /// </summary>
        [Description("INTERROMPIDA")]
        INTERROMPIDA = 4,
        /// <summary>
        /// 
        /// </summary>
        [Description("CANCELADA")]
        CANCELADA = 5,
        /// <summary>
        /// 
        /// </summary>
        [Description("AGENDADA")]
        AGENDADA = 6,
        /// <summary>
        /// 
        /// </summary>
        [Description("PENDENTE")]
        PENDENTE = 7,
        /// <summary>
        /// 
        /// </summary>
        [Description("PERDIDA")]
        PERDIDA = 8,
        /// <summary>
        /// 
        /// </summary>
        [Description("REEMBOLSO")]
        REEMBOLSO = 9,
        /// <summary>
        /// 
        /// </summary>
        [Description("REEMBOLSO ACEITO")]
        REEMBOLSO_ACEITO = 10,
        /// <summary>
        /// 
        /// </summary>
        [Description("REEMBOLSO RECUSADO")]
        REEMBOLSO_RECUSADO = 11,
        /// <summary>
        /// 
        /// </summary>
        [Description("RECUSADA DETRAN")]
        RECUSADA_DETRAN = 12,
        /// <summary>
        /// 
        /// </summary>
        [Description("AUTORIZADA DETRAN")]
        AUTORIZADA_DETRAN = 13,
        /// <summary>
        /// 
        /// </summary>
        [Description("CANCELADA USUARIO")]
        CANCELADA_USUARIO = 14,
        /// <summary>
        /// 
        /// </summary>
        [Description("REALIZADA NAO INTEGRADA")]
        REALIZADA_NAO_INTEGRADA = 30
    }
}