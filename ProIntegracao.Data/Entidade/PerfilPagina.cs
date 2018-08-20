using Prointegracao.Data.Entidade;

namespace ProIntegracao.Data.Entidade
{
    public class PerfilPagina : Entity
    {
        public virtual Perfil Perfil { get; set; }
        public virtual Pagina Pagina { get; set; }
        public virtual bool Inserir { get;set;}
        public virtual bool Atualizar { get;set;}
        public virtual bool Excluir { get;set;}
        public virtual bool Consultar { get;set;}
    }
}
