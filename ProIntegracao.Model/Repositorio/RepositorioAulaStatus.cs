using ProIntegracao.Data.EntidadeProSimulador;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioAulaStatus : BaseRepositorioProSimulador
    {

        /// <summary>
        /// Obter Todos
        /// </summary>
        /// <returns></returns>
        public List<AulaStatus> ObterTodos()
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            List<AulaStatus> lista = new List<AulaStatus>();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("SELECT [ID_AULA_STATUS],[STATUS] FROM TB_AULA_STATUS (NOLOCK) ORDER BY [STATUS]", conn);
                    command.CommandType = CommandType.Text;
                    dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        var uf = new AulaStatus()
                        {
                            Id          = Convert.ToInt32(dr["ID_AULA_STATUS"].ToString())
                            , Nome    = dr["STATUS"].ToString()
                        };

                        lista.Add(uf);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog(string.Format("REPOSITORIO STATUS AULA | Erro : {0}" , ex.Message),"erro");
            }

            return lista;
        }
    }
}
