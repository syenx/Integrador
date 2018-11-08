using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioSituacaoLogSimulador : BaseRepositorioProSimulador
    {
        public List<SituacaoLogSimulador> ListarSituacaoLogSimulador(string cpf, string dtInicio, string dtFim, string psa, int acao)
        {
            DateTime? dtInicial;
            DateTime? dtFinal;

            // Data Inicial
            if (string.IsNullOrEmpty(dtInicio))
                dtInicial = null;
            else
                dtInicial = Convert.ToDateTime(dtInicio);

            // Data Final
            if (string.IsNullOrEmpty(dtFim))
                dtFinal = null;
            else
                dtFinal = Convert.ToDateTime(dtFim);



            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);

            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            List<SituacaoLogSimulador> lista = new List<SituacaoLogSimulador>();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (dtInicial == DateTime.Today)
                    {
                        command = new SqlCommand("pr_selecionar_xml_log_aula_dia", conn);
                        command.Parameters.AddWithValue("@DETALHES", cpf);
                    }
                    else {
                        command = new SqlCommand("pr_selecionar_xml_log_aula_historico", conn);
                        command.Parameters.AddWithValue("@DETALHES", cpf);
                        command.Parameters.AddWithValue("@DTINICIO", dtInicial);
                        command.Parameters.AddWithValue("@DTFINAL", dtFinal);
                        
                    }

                    command.Parameters.AddWithValue("@PSA", psa);
                    command.Parameters.AddWithValue("@ACAO", acao);
                    
                    command.CommandType = CommandType.StoredProcedure;
                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var situacao = new SituacaoLogSimulador()
                        {
                            UF              = dr["UF"].ToString()
                            , IdEmpresa     = Convert.ToInt32(dr["ID_EMPRESA"].ToString())
                            , Codigo        = Convert.ToInt32(dr["CODIGO"].ToString())
                            , RazaoSocial   = dr["RAZAO_SOCIAL"].ToString()
                            , NomeFantasia  = dr["NOME_FANTASIA"].ToString()
                            , DataCadastro  = Convert.ToDateTime(dr["DATA_CADASTRO"].ToString())
                            , Acao          = dr["ACAO"].ToString()
                            , Detalhes      = dr["DETALHES"].ToString()
                            , IP            = dr["IP"].ToString()
                            , XmlEntrada    = dr["XML_ENTRADA"].ToString()
                            , XmlSaida      = dr["XML_SAIDA"].ToString()
                        };

                        lista.Add(situacao);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO SITUACAO LOG SIMULADOR", "Erro de Consulta | Messagem : " + ex.Message);
            }
            
            return lista;
        }
    }
}
