using ProIntegracao.Model;


namespace Prointegracao.Model.Entidade
{
    public class PerfilPagina : Entity
    {
        public virtual Perfil Perfil { get; set; }
        public virtual Pagina Pagina { get; set; }
        public virtual bool INSERIR { get;set;}
        public virtual bool ATUALIZAR { get;set;}
        public virtual bool DELETAR { get;set;}
        public virtual bool VISUALIZAR { get;set;}
    }
}
