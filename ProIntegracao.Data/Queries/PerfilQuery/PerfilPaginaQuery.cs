using ProIntegracao.Data.Entidade;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
//atual
namespace ProIntegracao.Data
{
    public class PerfilPaginaQuery
    {

        private static readonly RepositorioBase _repo = new RepositorioBase();


        public static NHibernateSessionManager Session
        {
            get
            {
                return NHibernateSessionManager.Instance;
            }
        }

        public static List<PerfilPagina> ConsultarPerfilPaginas()
        {
            return new RepositorioBase().ObterTodos<PerfilPagina>().ToList();
        }


        public static List<PerfilPagina> ConsultarPerfilPaginasPorEstados(int[] Estados)
        {
            var denoms = new HashSet<int>(Estados);
            //var model = _repo.ObterTodos<PerfilPagina>().Where( a => denoms.Contains(a.Pagina.Id)).ToList();

            return new List<PerfilPagina>().ToList();
        }


        public static List<PerfilPagina> ConsultarMenuInativo()
        {
            return new RepositorioBase().ObterTodos<PerfilPagina>().Where(f => f.Id == 0).ToList();
        }
        public static List<PerfilPagina> ObterComLeft()
        {
            // ICriteria criteria = RepositorioBase.Session.GetSession().CreateCriteria(typeof(PerfilPagina));
            // criteria.CreateAlias("PefilPagina", "pp");
            //// criteria.CreateAlias("Pagina", "pa", JoinType.LeftOuterJoin);
            // //criteria.Add(Restrictions.IsNull(Projections.Property<PerfilPagina>(p => p.Pagina.ID_PAGINA)));
            // return criteria.List<PerfilPagina>();
            List<PerfilPagina> result1 = new List<PerfilPagina>();


            var query = "SELECT Distinct a.ID_PAGINA, a.ID_MENU ,a.NOME,a.URL, A.ATIVO ,b.ATUALIZAR,DELETAR, VISUALIZAR,INSERIR  FROM TB_PAGINA AS A LEFT JOIN TB_PERFIL_PAGINA AS B on a.ID_PAGINA = b.ID_PAGINA AND B.ID_PAGINA IS NULL";

            var session = Session.GetSession();
            var result = session.CreateSQLQuery(query).List();
            foreach (var item in result)
            {

                var perfil = new PerfilPagina()
                {
                    Pagina = pagina((Convert.ToInt32((((object[])item)[0]))),
                                  Convert.ToInt32(((object[])item)[1]),
                                   Convert.ToString(((object[])item)[2]),
                                   Convert.ToString(((object[])item)[3]),
                                   Convert.ToBoolean(((object[])item)[4])),

                    Atualizar = Convert.ToBoolean(((object[])item)[5]),
                    Excluir = Convert.ToBoolean(((object[])item)[6]),
                    Consultar = Convert.ToBoolean(((object[])item)[7]),
                    Inserir = Convert.ToBoolean(((object[])item)[8]),
                };

                result1.Add(perfil);
            }

            return result1;

            // var query = new RepositorioBase().ObterTodos<PerfilPagina>().Where(a => a.Pagina.ID_PAGINA is null)
        }

        public static int ObterUltimoIdInserido()
        {
            var query = "select top 1 ID_PERFIL from TB_PERFIL order by Id_Perfil desc";
            SqlConnection con = new SqlConnection(@"Data Source = 189.36.10.90, 49433; Initial Catalog = ProIntegracao; Persist Security Info = True; User ID = mfranco; Password = M@rcelo88");
            SqlCommand command = new SqlCommand(query, con);
            con.Open();

            var sqldatareader = ObterTabela(command.ExecuteReader());
            return int.Parse(sqldatareader.Rows[0].ItemArray[0].ToString());
        }

        public static IList<PerfilPagina> ObterTodos()
        {
            SqlConnection con = new SqlConnection(@"Data Source = 189.36.10.90, 49433; Initial Catalog = ProIntegracao; Persist Security Info = True; User ID = mfranco; Password = M@rcelo1988");
            SqlCommand command = new SqlCommand("SELECT Distinct a.IdPagina, a.IdMenu ,a.NOME,a.URL, ,b.Atualizar,Excuir, Consultar,Inserir  FROM TbPagina AS A LEFT JOIN TbPerfilPagina AS B on a.IdPagina = b.IdPagina AND B.IdPagina IS NULL", con);
            con.Open();

            PerfilPagina perfilPagina = new PerfilPagina();
            List<PerfilPagina> listaPerfilPagina = new List<PerfilPagina>();

            var sqldatareader = ObterTabela(command.ExecuteReader());
            try
            {
                for (int i = 0; i < sqldatareader.Select().Count(); i++)
                {
                    if (!sqldatareader.Select().Count().Equals(null))
                    {
                        for (int j = 0; j < sqldatareader.Select()[i].ItemArray.Count(); j++)
                        {


                            perfilPagina.Pagina = pagina(int.Parse(sqldatareader.Select()[i].ItemArray[0].ToString()),
                                   int.Parse(sqldatareader.Select()[i].ItemArray[1].ToString()),
                                   sqldatareader.Select()[i].ItemArray[2].ToString(),
                                   sqldatareader.Select()[i].ItemArray[3].ToString(),
                                   Convert.ToBoolean(sqldatareader.Select()[i].ItemArray[4].ToString()));
                            perfilPagina.Consultar = false;
                            perfilPagina.Excluir = false;
                            perfilPagina.Atualizar = false;
                            perfilPagina.Inserir = false;
                        }

                    }
                    listaPerfilPagina.Add(perfilPagina);

                }
            }
            catch
            {

                return listaPerfilPagina;
            }
            return listaPerfilPagina;
        }
        public static Pagina pagina(int _ID_PAGINA, int _ID_MENU, string _URL, string _NOME, bool _ATIVO)
        {

            int ID_PAGINA = _ID_PAGINA;
            int ID_MENU = _ID_MENU;
            string URL = _URL;
            string NOME = _NOME;
            bool ATIVO = _ATIVO;

            var perfilnovo = new Pagina()
            {
                Id= ID_PAGINA,
                //ID_MENU = ID_MENU,
                Url = URL,
                Nome = NOME,
               // ATIVO = ATIVO
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
        public static PerfilPagina ConsultarPorID(int idMenu)
        {
            return new RepositorioBase().ProcurarTodos<PerfilPagina>(x => x.Id == idMenu).FirstOrDefault();
        }


        


        public static IList<PerfilPagina> ConsultarListaPorID(int idPerfilPagina)
        {
            return new RepositorioBase().ProcurarTodos<PerfilPagina>(x => x.Id == idPerfilPagina).ToList<PerfilPagina>();
        }
        public static void Inserir(PerfilPagina perfilPagina)
        {
            new RepositorioBase().Salvar(perfilPagina);
        }


    }
}
