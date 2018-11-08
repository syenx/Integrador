using System;
using System.Collections.Generic;
using ProIntegracao.Data;
using System.Linq;
using ProIntegracao.Model.Interface;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using ProIntegracao.Data.Entidade;

namespace ProIntegracao.Model.Repositorio.Base
{
    /// <summary>
    /// Base Repositorio 
    /// </summary>
    /// <typeparam name="T">Entidade</typeparam>
    public class BaseRepositorio<T> : IBaseRepositorio<T>
    {
        #region Variáveis

        public static NHibernateSessionManager Session { get { return NHibernateSessionManager.Instance; } }

        #endregion

        #region Métodos Públicos

        /// <summary> Listar Entidades
        /// </summary>
        /// <returns></returns>
        public IList<T> Listar()
        {
            return Session.GetSession().CreateCriteria(typeof(T)).List<T>().ToList();
        }

        /// <summary> Atualizar
        /// </summary>
        /// <param name="entidade">Entidade T</param>
        public bool Atualizar(T entidade)
        {
            try
            {
                BeginTransaction();
                Session.GetSession().SaveOrUpdate(entidade);
                CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                RollBackTransaction();
                //TODO : log
                return false;
            }

        }




        /// <summary> Atualizar
        /// </summary>
        /// <param name="entidade">Entidade T</param>
        public bool AtualizarSemTransacao(T entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException();

            try
            {
                Session.GetSession().SaveOrUpdate(entidade);
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                return false;
            }

            return true;

        }



        /// <summary> Excluri Entidade
        /// </summary>
        /// <param name="entidade">Entidade</param>
        public void Excluir(T entidade)
        {
            try
            {
                Session.BeginTransaction();
                Session.GetSession().Delete(entidade);
                Session.CommitTransaction();
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                Session.RollbackTransaction();
            }


        }

        /// <summary>Salvar
        /// </summary>
        /// <param name="entidade">Entitdade T</param>
        /// <returns></returns>
        public int Salvar(T entidade)
        {
            try
            {
                Session.BeginTransaction();
                var retorno = Session.GetSession().Save(entidade);
                Session.CommitTransaction();

                if (retorno != null)
                {

                    return (int)retorno;

                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                
                Session.RollbackTransaction();
                var msgErro = ex.Message;
                return 0;
            }

        }

        #endregion

        #region Metodos Privados

        /// <summary>
        /// RollBackTransaction
        /// </summary>
        public void RollBackTransaction()
        {
            Session.RollbackTransaction();
        }

        /// <summary>
        /// CommitChanges
        /// </summary>
        protected void CommitChanges()
        {
            if (!Session.HasOpenTransation())
            {
                Session.GetSession().Flush();
            }
        }

        /// <summary>
        /// BeginTransaction
        /// </summary>
        public void BeginTransaction()
        {
            if (!Session.HasOpenTransation())
                Session.BeginTransaction();
        }

        /// <summary>
        /// CommitTransaction
        /// </summary>
        public void CommitTransaction()
        {
            Session.CommitTransaction();
        }

#pragma warning disable CS0693 // O parâmetro de tipo tem o mesmo nome que o parâmetro de tipo do tipo externo
                              /// <summary>
                              /// Obter Entidade
                              /// </summary>
                              /// <typeparam name="T">Entidade</typeparam>
                              /// <param name="Id">IdEnitdade</param>
                              /// <returns></returns>
        public T Obter<T>(int Id)
#pragma warning restore CS0693 // O parâmetro de tipo tem o mesmo nome que o parâmetro de tipo do tipo externo
        {
            return ProcurarUm<T>(new { Id = Id });

        }

#pragma warning disable CS0693 // O parâmetro de tipo tem o mesmo nome que o parâmetro de tipo do tipo externo
        /// <summary>
        /// Procurar por Um
        /// </summary>
        /// <typeparam name="T">Entidade</typeparam>
        /// <param name="parametros">Parametros</param>
        /// <returns></returns>
        public T ProcurarUm<T>(object parametros)
#pragma warning restore CS0693 // O parâmetro de tipo tem o mesmo nome que o parâmetro de tipo do tipo externo
        {
            ICriteria criteria = AdicionaRestricoes(parametros, typeof(T), false);
            return criteria.UniqueResult<T>();
        }

        /// <summary>
        /// Adicionar Restrições
        /// </summary>
        /// <param name="properties">properties</param>
        /// <param name="type">type</param>
        /// <param name="like">like</param>
        /// <returns></returns>
        protected ICriteria AdicionaRestricoes(object properties, Type type, bool like)
        {
            ICriteria criteria = Session.GetSession().CreateCriteria(type);
            SimpleExpression comparacao = null;

            foreach (var property in ParametrosParaDicionario(properties))
            {
                if (property.Key.IndexOf('.') > 0)
                {
                    var classes = property.Key.Substring(0, property.Key.LastIndexOf('.'));
                    criteria.CreateAlias(classes, classes, JoinType.InnerJoin);
                }

                if (property.Key.IndexOf(' ') > 0)
                {
                    var operador = property.Key.Substring(property.Key.LastIndexOf(' '));

                    if (">=".Equals(operador.Trim()))
                    {
                        comparacao = Restrictions.Ge(property.Key.Replace(operador, "").Trim(), property.Value);
                    }
                    else if ("<=".Equals(operador.Trim()))
                    {
                        comparacao = Restrictions.Le(property.Key.Replace(operador, "").Trim(), property.Value);
                    }
                }
                else
                {
                    comparacao = like && property.Value is String
                                     ? Restrictions.Like(property.Key, property.Value.ToString(), MatchMode.Anywhere)
                                     : Restrictions.Eq(property.Key, property.Value);
                }

                criteria.Add(property.Value != null
                        ? comparacao
                        : Restrictions.IsNull(property.Key));
            }

            return criteria;
        }

        /// <summary>
        /// Parametros Para Dicionário
        /// </summary>
        /// <param name="parametros">Parametros</param>
        /// <returns></returns>
        protected IDictionary<string, object> ParametrosParaDicionario(object parametros)
        {
            if (parametros == null) return new Dictionary<string, object>();
            if (parametros is IDictionary<string, object>) return (IDictionary<string, object>)parametros;
            return parametros.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(parametros, null));
        }



        #endregion
    }
}
