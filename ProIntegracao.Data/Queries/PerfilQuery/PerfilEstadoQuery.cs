using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProIntegracao.Data
{
    public class PerfilEstadoQuery
    {
        public static NHibernateSessionManager Session
        {
            get
            {
                return NHibernateSessionManager.Instance;
            }
        }
        public static List<PerfilEstado> ConsultarPerfilPaginas()
        {
            return new RepositorioBase().ObterTodos<PerfilEstado>().ToList();
        }
        public static List<PerfilEstado> ConsultarMenuInativo()
        {
            return new RepositorioBase().ObterTodos<PerfilEstado>().Where(f => f.Id == 0).ToList();
        }
       
        public static Pagina pagina(int _ID_PAGINA, int _ID_MENU, string _URL, string _NOME, bool _ATIVO)
        {

            int ID_PAGINA = _ID_PAGINA;
            int ID_MENU = _ID_MENU;
            string URL = _URL;
            string NOME = _NOME;
           // bool ATIVO = _ATIVO;

            var perfilnovo = new Pagina()
            {
                Id = ID_PAGINA,
                //ID_MENU = ID_MENU,
                Url = URL,
                Nome = NOME,
                //ATIVO = ATIVO
            };

            return perfilnovo;
        }
        public static DataTable ObterTabela(SqlDataReader reader)
        {
            DataTable tbEsquema = reader.GetSchemaTable();
            DataTable tbRetorno = new DataTable();
            foreach (DataRow r in tbEsquema.Rows)
            {
                if (!tbRetorno.Columns.Contains(r["ColumnName"].ToString()))
                {
                    DataColumn col = new DataColumn()
                    {
                        ColumnName = r["ColumnName"].ToString(),
                        Unique = Convert.ToBoolean(r["IsUnique"]),
                        AllowDBNull = Convert.ToBoolean(r["AllowDBNull"]),
                        ReadOnly = Convert.ToBoolean(r["IsReadOnly"])
                    };
                    tbRetorno.Columns.Add(col);
                }
            }
            while (reader.Read())
            {
                DataRow novaLinha = tbRetorno.NewRow();
                for (int i = 0; i < tbRetorno.Columns.Count; i++)
                {
                    novaLinha[i] = reader.GetValue(i);
                }
                tbRetorno.Rows.Add(novaLinha);
            }
            return tbRetorno;
        }
        public static PerfilEstado ConsultarMatriculaPorID(int idMenu)
        {
            return new RepositorioBase().ProcurarTodos<PerfilEstado>(x => x.Id == idMenu).FirstOrDefault();
        }
        public static void Inserir(PerfilEstado perfilPagina)
        {
            new RepositorioBase().Salvar(perfilPagina);
        }

      
    }
}
