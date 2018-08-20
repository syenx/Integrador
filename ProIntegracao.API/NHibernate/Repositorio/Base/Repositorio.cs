using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using System.Linq.Expressions;

namespace ProIntegracao.API
{
    public class Repositorio<T> : IRepositorio<T>
    {
        public static NHibernateSessionManager Session
        {
            get
            {
                return NHibernateSessionManager.Instance;
            }
        }
        public IList<T> Listar()
        {
            return Session.GetSession().CreateCriteria(typeof(T)).List<T>().ToList();
        }
        public bool Atualizar(T entidade)
        {
            try
            {
                BeginTransaction();
                Session.GetSession().SaveOrUpdate(entidade);
                CommitTransaction();
                return true;
            }
            catch (Exception ex){

                RollBackTransaction();
                //TODO : log
                return false;
            }

        }
        public void Excluir(T entidade)
        {
            Session.GetSession().Delete(entidade);
        }
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
            catch (Exception rx)
            {
                Session.RollbackTransaction();
                return 0 ;
            }

        }
    
        public IList<T> ObterComLeft<T>(string firstEntity, string SecondName, string type)
        {
            ICriteria criteria = Session.GetSession().CreateCriteria(type);

            criteria.CreateAlias(firstEntity, "entity1", JoinType.LeftOuterJoin); // inner join by default
            criteria.CreateAlias(SecondName, "entity2");
            criteria.Add(Restrictions.IsNull(Projections.Property<T>(a => a)));

            return criteria.List<T>();
        }

    
        protected IDictionary<string, object> ParametrosParaDicionario(object parametros)
        {
            if (parametros == null) return new Dictionary<string, object>();
            if (parametros is IDictionary<string, object>) return (IDictionary<string, object>)parametros;
            return parametros.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(parametros, null));
        }
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

        public IList<T> ProcurarTodos<T>(object parametros, bool searchWithLike) where T : Entity
        {
            ICriteria criteria = AdicionaRestricoes(parametros, typeof(T), searchWithLike);
            return criteria.List<T>();
        }

        public IList<T> ProcurarTodos<T>(object parametros) where T : Entity
        {
            return ProcurarTodos<T>(parametros, false);
        }

        
        public IList<T> ProcurarTodos<T>(Expression<Func<T, bool>> expression) where T : Entity
        {
            return Session.GetSession().CreateCriteria(typeof(T)).Add(Restrictions.Where<T>(expression)).List<T>();
        }

        internal static IList<T> Procurar<T>(ICriterion[] expressions, Order[] orders) where T : Entity
        {
            var criteria = Session.GetSession().CreateCriteria(typeof(T));

            if (expressions != null)
                foreach (var expression in expressions)
                    criteria.Add(expression);

            if (orders != null)
                foreach (var order in orders)
                    criteria.AddOrder(order);

            return criteria.List<T>();
        }

        public void Salvar(Entity entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException();
            try
            {
                BeginTransaction();
                Session.GetSession().SaveOrUpdate(entidade);
                CommitTransaction();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }


        public int SalvarSemTransacao(Entity entidade)
        {

            if (entidade == null)
                throw new ArgumentNullException();

            try
            {
                Session.GetSession().SaveOrUpdate(entidade);
            }
            catch (Exception ex)
            {
                return 0;
            }

            return entidade.Id;

        }

        public void NewAtualizar(Entity entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException();

            try
            {
                BeginTransaction();
                Session.GetSession().Merge(entidade);
                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollBackTransaction();
                throw;
            }
        }

        public void Atualizar(Entity entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException();

            try
            {
                BeginTransaction();

                Session.GetSession().SaveOrUpdate(entidade);
                CommitTransaction();
            }
            catch
            {
                RollBackTransaction();
                throw;
            }
        }


        public void SalvarComTransaction(Entity entidade)
        {
            if (entidade == null)
                throw new ArgumentNullException();

            try
            {

                Session.GetSession().SaveOrUpdate(entidade);
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        public void Salvar(Entity[] entidades)
        {
            if (entidades == null || entidades.Count() == 0)
                throw new ArgumentNullException();

            try
            {
                BeginTransaction();

                foreach (var entidade in entidades)
                {

                    Session.GetSession().SaveOrUpdate(entidade);
                }

                CommitTransaction();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        public void SalvarComTransaction(Entity[] entidades)
        {
            if (entidades == null || entidades.Count() == 0)
                throw new ArgumentNullException();

            try
            {

                foreach (var entidade in entidades)
                {

                    Session.GetSession().SaveOrUpdate(entidade);
                }

            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        public void Deletar(Entity entidade)
        {
            Session.GetSession().Delete(entidade);
            CommitChanges();
        }

        public void Deletar(List<Entity> entidades)
        {
            try
            {
                BeginTransaction();

                foreach (var entidade in entidades)
                    Session.GetSession().Delete(entidade);

                CommitTransaction();
            }
            catch (Exception)
            {
                RollBackTransaction();
                throw;
            }
        }

        public void BeginTransaction()
        {
            if (!Session.HasOpenTransation())
                Session.BeginTransaction();
        }

        public void CommitTransaction()
        {
            Session.CommitTransaction();
        }

        public void RollBackTransaction()
        {
            Session.RollbackTransaction();
        }

        protected void CommitChanges()
        {
            if (!Session.HasOpenTransation())
            {
                Session.GetSession().Flush();
            }
        }
        
        public T ObterPorChave<T>(int Id) where T : Entity
        {
            return ProcurarUm<T>(new { Id = Id });
        }
        public T ProcurarUm<T>(object parametros) where T : Entity
        {
            ICriteria criteria = AdicionaRestricoes(parametros, typeof(T), false);
            return criteria.UniqueResult<T>();
        }
        public T ProcurarUm<T>(Expression<Func<T, bool>> expression) where T : Entity
        {
            return Session.GetSession().CreateCriteria(typeof(T)).Add(Restrictions.Where<T>(expression)).UniqueResult<T>();
        }

        public IList<T> ObterListaDesc<T>(string descString)
        {
            return Session.GetSession().CreateCriteria(typeof(T)).AddOrder(Order.Desc(descString)).List<T>();
        }

        public IList<T> ObterTodos<T>() where T : Entity
        {
            return Session.GetSession().CreateCriteria(typeof(T)).List<T>();
        }

        public void ProcurarTodos<T1>()
        {
            throw new NotImplementedException();
        }

        public T1 Obter<T1>(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
