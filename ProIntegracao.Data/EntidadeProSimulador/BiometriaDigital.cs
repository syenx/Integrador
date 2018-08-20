using System;

namespace ProIntegracao.Data.EntidadeProSimulador
{
    public class BiometriaDigital : Entity
    {
        public virtual int Dedo { get; set; }
        public virtual string Template { get; set; }
        public virtual string Imagem { get; set; }
        public virtual DateTime DataCadastro { get; set; }
    }
}
