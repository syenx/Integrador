using ProIntegracao.Data.EntidadeProSimulador;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioUF : BaseRepositorioProSimulador
    {
        /// <summary>
        /// Obter Lista
        /// </summary>
        /// <returns>Obter Lista</returns>
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
                    command = new SqlCommand("SELECT ID_UF, SIGLA FROM TB_UF (NOLOCK) ORDER BY SIGLA", conn);
                    command.CommandType = CommandType.Text;
                    dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        var uf = new AulaStatus()
                        {
                            Id      = Convert.ToInt32(dr["ID_UF"].ToString())
                            , Nome   = dr["SIGLA"].ToString()
                        };

                        lista.Add(uf);
                    }
                }
            }
            catch (Exception ex)
            {
                //InserirLog("REPOSITORIO SITUACAO AULA", ex.Message);
            }

            return lista;
        }

    }
}
