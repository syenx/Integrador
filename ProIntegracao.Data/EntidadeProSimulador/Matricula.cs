using System;
namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class Matricula
    {
        public int IdMatricula { get; set; }

        public string CodigoMatricula { get; set; }

        public int QtdAulas { get; set; }

        public DateTime DataMatricula { get; set; }

        public string Usuario { get; set; }

        public string IdEmpresa { get; set; }

        public string CodigoCFC { get; set; }

        public string CFCOrigem { get; set; }

        public string CNPJ { get; set; }

        public string Status { get; set; }

    }
}
