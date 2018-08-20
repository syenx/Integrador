using ProIntegracao.Data;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProIntegracao.Model.Interface
{
    public interface IBaseRepositorioProSimulador
    {
        IList<T> ProcurarTodos<T>(object parametros, bool searchWithLike) where T : Entity;
        IList<T> ProcurarTodos<T>(object parametros) where T : Entity;
        IList<T> ProcurarTodos<T>(Expression<Func<T, bool>> expression) where T : Entity;
        void Atualizar(Entity entidade);
        void Salvar(Entity[] entidade);
        void SalvarComTransaction(Entity entidade);
        void SalvarComTransaction(Entity[] entidade);
        void Deletar(Entity entidade);
        void Deletar(List<Entity> entidades);
        void BeginTransaction();
        void CommitTransaction();
        void RollBackTransaction();
        T ObterPorChave<T>(int id) where T : Entity;
        T ProcurarUm<T>(object parametros) where T : Entity;
        T ProcurarUm<T>(Expression<Func<T, bool>> expression) where T : Entity;
        IList<T> ObterTodos<T>() where T : Entity;
    }
}
