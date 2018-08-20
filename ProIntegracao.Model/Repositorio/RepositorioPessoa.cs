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
    public class RepositorioPessoa : BaseRepositorioProSimulador
    {
        public List<Pessoa> ListarAlunosDuplicados(string cpf, string nome, string renach)
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            cpf = cpf.Replace(".", "").Replace("-", "");

            //TESTE
            //cpf = "00965190021";

            List<Pessoa> lista = new List<Pessoa>();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    command = new SqlCommand("pr_selecionar_alunos_duplicados", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CPF", cpf);
                    command.Parameters.AddWithValue("@NOME", nome);
                    command.Parameters.AddWithValue("@RENACH", renach);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var pessoa = new Pessoa()
                        {
                            CFC             = dr["CFC"].ToString()
                            , RazaoSocial   = dr["RazaoSocial"].ToString()
                            , CPF           = dr["CPF"].ToString()
                            , Nome          = dr["Nome"].ToString()
                            , Status        = dr["Status"].ToString()
                            , Tipo          = dr["TIPO"].ToString()
                        };

                        lista.Add(pessoa);
                    }
                }
            }
            catch (Exception ex)
            {
                //InserirLog("REPOSITORIO ALUNO DUPLICADO", ex.Message);
            }

            return lista;
        }


    }
}
