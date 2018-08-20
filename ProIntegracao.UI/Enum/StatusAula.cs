using System.ComponentModel;
namespace ProIntegracao.UI.Enum
{
    public enum StatusAula
    {
        [Description("INICIADA")]
        INICIADA = 1,

        [Description("EM ANDAMENTO")]
        EM_ANDAMENTO = 2,

        [Description("REALIZADA")]
        REALIZADA = 3,

        [Description("INTERROMPIDA")]
        INTERROMPIDA = 4,

        [Description("CANCELADA")]
        CANCELADA = 5,

        [Description("AGENDADA")]
        AGENDADA = 6,

        [Description("PENDENTE")]
        PENDENTE = 7,

        [Description("PERDIDA")]
        PERDIDA = 8,

        [Description("REEMBOLSO")]
        REEMBOLSO = 9,

        [Description("REEMBOLSO ACEITO")]
        REEMBOLSO_ACEITO = 10,

        [Description("REEMBOLSO RECUSADO")]
        REEMBOLSO_RECUSADO = 11,

        [Description("RECUSADA DETRAN")]
        RECUSADA_DETRAN = 12,

        [Description("AUTORIZADA DETRAN")]
        AUTORIZADA_DETRAN = 13,

        [Description("CANCELADA USUARIO")]
        CANCELADA_USUARIO = 14,

        [Description("REALIZADA NAO INTEGRADA")]
        REALIZADA_NAO_INTEGRADA = 30
    }
}