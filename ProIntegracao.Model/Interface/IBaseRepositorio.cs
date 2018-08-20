using System.Collections.Generic;

namespace ProIntegracao.Model.Interface
{
    public interface IBaseRepositorio<T>
    {
        /// <summary>
        /// Listar
        /// </summary>
        /// <returns></returns>
        IList<T> Listar();

        /// <summary>
        /// Obter
        /// </summary>
        /// <typeparam name="T">Entidade</typeparam>
        /// <param name="Id">Id</param>
        /// <returns></returns>
        T Obter<T>(int Id);

        /// <summary>
        /// Atualizar
        /// </summary>
        /// <param name="entidade">Entidade</param>
        /// <returns></returns>
        bool Atualizar(T entidade);

        /// <summary>
        /// Salvar
        /// </summary>
        /// <param name="entidade">Entidade</param>
        /// <returns></returns>
        int Salvar(T entidade);

        /// <summary>
        /// Excluir
        /// </summary>
        /// <param name="entidade">Entidade</param>
        void Excluir(T entidade);

        /// <summary>
        /// BeginTransaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Commit Transcation
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// RollBack Transactions
        /// </summary>
        void RollBackTransaction();
    }
}
