using System;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class HistoricoAluno
    {
        #region propriedades

        public string Nome { get; set; }

        public DateTime? DataNascimento { get; set; }

        public string CPF { get; set; }
        
        public string Renach { get; set; }
        
        public string Curso { get; set; }

        public string Status { get; set; }

        public string Biometria { get; set; }

        public string CEP { get; set; }

        public string Endereco { get; set; }
        
        public string Bairro { get; set; }


        public string Cidade { get; set; }

        public string Estado { get; set; }
        
        public string Telefone { get; set; }

        public string Celular { get; set; }
        
        public string IdAluno { get; set; }

        #endregion
    }
}
