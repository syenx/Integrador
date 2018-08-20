using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using System;
using System.Configuration;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace ProIntegracao.Data.Conf
{
    public abstract class SessionManagerProSimulador
    {
        private ISessionFactory _sessionFactory;

        public abstract string GetTransactionKey();
        public abstract string GetSessionKey();

        protected SessionManagerProSimulador()
        {
            InitSessionFactory();
        }

        private ITransaction CurrentTransaction
        {
            get
            {
                if (IsInWebContext())
                {
                    return (ITransaction)HttpContext.Current.Items[GetTransactionKey()];
                }

                return (ITransaction)CallContext.GetData(GetTransactionKey());
            }
            set
            {
                if (IsInWebContext())
                {
                    HttpContext.Current.Items[GetTransactionKey()] = value;
                }
                else
                {
                    CallContext.SetData(GetTransactionKey(), value);
                }
            }
        }

        private ISession CurrentSession
        {
            get
            {
                if (IsInWebContext())
                {
                    return (ISession)HttpContext.Current.Items[GetSessionKey()];
                }
                else
                {
                    return (ISession)CallContext.GetData(GetSessionKey());
                }
            }
            set
            {
                if (IsInWebContext())
                {
                    HttpContext.Current.Items[GetSessionKey()] = value;
                }
                else
                {
                    CallContext.SetData(GetSessionKey(), value);
                }
            }
        }

        private bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }

        private void InitSessionFactory()
        {
            var mapper = new ModelMapper();

            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());

            var mapeamentoDominio = mapper.CompileMappingForAllExplicitlyAddedEntities();


            //var testStrinConecao = "Data Source=189.36.10.90,49433;Initial Catalog=ProIntegracao;Persist Security Info=True;User ID=mfranco;Password=M@rcelo1988"; //System.Configuration.ConfigurationSettings.AppSettings["ConexaoSQL.0"];


            var configuration = new NHibernate.Cfg.Configuration();
            configuration.DataBaseIntegration(c =>
            {
                c.Dialect<MsSql2008Dialect>();
                c.ConnectionString = ObterStringConexao();//System.Configuration.ConfigurationSettings.AppSettings["ConexaoSQL.0"];
                c.KeywordsAutoImport = Hbm2DDLKeyWords.AutoQuote;
            }).AddMapping(mapeamentoDominio);

            configuration.AddAssembly("ProIntegracao.Data");

            _sessionFactory = configuration.BuildSessionFactory();
        }

        public ISession GetSession()
        {
            if (CurrentSession == null)
            {
                CurrentSession = _sessionFactory.OpenSession();
            }

            if (!CurrentSession.IsOpen) CurrentSession.Reconnect();

            return CurrentSession;
        }

        public void CloseSession()
        {
            if (CurrentSession != null && CurrentSession.IsOpen)
            {
                CurrentSession.Close();
            }
            CurrentSession = null;
        }

        public void BeginTransaction()
        {
            if (CurrentTransaction != null)
            {
                throw new Exception("Transação já aberta");
            }

            CurrentTransaction = GetSession().BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (!HasOpenTransation())
            {
                throw new Exception("Não existe transação aberta");
            }

            try
            {
                CurrentSession.Flush();
                CurrentTransaction.Commit();
            }
            catch (HibernateException)
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                CurrentTransaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (!HasOpenTransation())
            {
                throw new Exception("Não existe transação aberta");
            }

            try
            {
                CurrentTransaction.Rollback();
                CurrentTransaction = null;
            }
            finally
            {
                CloseSession();
            }
        }

        public bool HasOpenTransation()
        {
            return CurrentTransaction != null && !CurrentTransaction.WasCommitted && !CurrentTransaction.WasRolledBack;
        }

        public string ObterStringConexao()
        {
            return TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
        }

    }
}
