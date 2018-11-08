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

#pragma warning disable CS0693 // O parâmetro de tipo tem o mesmo nome que o parâmetro de tipo do tipo externo
                              /// <summary>
                              /// 
                              /// </summary>
                              /// <typeparam name="T"></typeparam>
                              /// <param name="Id"></param>
                              /// <returns></returns>
        T Obter<T>(int Id);
#pragma warning restore CS0693 // O parâmetro de tipo tem o mesmo nome que o parâmetro de tipo do tipo externo

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
