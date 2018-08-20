using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProIntegracao.Data
{
    public class NHibernateSessionManager :SessionManager
    {
        #region Thread-safe, lazy Singleton

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
        public static NHibernateSessionManager Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
            }
            set
            {
                Nested.NHibernateSessionManager = value;
            }
        }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        public class Nested
        {
            static Nested() { }
            public static NHibernateSessionManager NHibernateSessionManager =
               new NHibernateSessionManager();
        }

        #endregion

        public override string GetTransactionKey()
        {
            return "CONTEXT_TRANSACTION";
        }

        public override string GetSessionKey()
        {
            return "CONTEXT_SESSION";
        }
    }
}
