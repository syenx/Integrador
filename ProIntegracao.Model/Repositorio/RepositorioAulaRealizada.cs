using ProIntegracao.Data.Entidade;
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
    public class RepositorioAulaRealizada : BaseRepositorio<AulaRealizada>
    {

        private RepositorioConfiguracao _repoConfig = new RepositorioConfiguracao();

        public List<AulaRealizada> ListarAulaRealizada(string dtInicio, string dtFim, string idEstado)
        {
            
            var lista = new List<AulaRealizada>();

            if (string.IsNullOrEmpty(dtInicio))
                return lista;
            
            var dataInicial     = Convert.ToDateTime(dtInicio);
            var dataFinal       = (dtFim == "") ? Convert.ToDateTime(dtInicio) : Convert.ToDateTime(dtFim);

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProIntegracao"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("SpRelAulaRealizada", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DataInicio", dataInicial);
                    command.Parameters.AddWithValue("@DataFim", dataFinal);
                    command.Parameters.AddWithValue("@UF", (!string.IsNullOrEmpty(idEstado)) ? idEstado : null);
                    
                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var aularealizada = new AulaRealizada()
                        {
                            Data                    = Convert.ToDateTime(dr["Data"].ToString())
                            , SMC                   = dr["CodigoSMC"].ToString()
                            , CNPJ                  = dr["CNPJ"].ToString()
                            , RazaoSocial           = dr["RazaoSocial"].ToString()
                            , UF                    = dr["UF"].ToString()
                            , AulasRealizadas       = Convert.ToInt32(dr["AjusteRealizada"].ToString())
                        };
                        lista.Add(aularealizada);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
            }
            
            return lista.OrderBy(m=>m.UF).OrderBy(m => m.Data).ToList();
        }


 
        /// <summary>
        /// Listar Contratos Firmados
        /// </summary>
        /// <param name="dtInicio">Dt Inicio</param>
        /// <param name="idEstado">Id Estado</param>
        /// <returns></returns>
        public List<ContratosFirmados> ListarContratosFirmados(string dtInicio, string idEstado)
        {

            var lista = new List<ContratosFirmados>();

            if (string.IsNullOrEmpty(dtInicio))
                return lista;

            var dataInicial = Convert.ToDateTime(dtInicio);
           
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProIntegracao"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("SpRelContrato", conn);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@DataFim", dataInicial);
                    command.Parameters.AddWithValue("@UF", (!string.IsNullOrEmpty(idEstado)) ? idEstado : null);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var contrato = new ContratosFirmados()
                        {
                            Data = Convert.ToDateTime(dr["Data"].ToString())
                            ,Estado = dr["UF"].ToString()
                            ,NrContratos = Convert.ToInt32(dr["Contratos"].ToString())
                            ,NrSimuladoresContratados = Convert.ToInt32(dr["Simuladores"].ToString())
                        };

                        lista.Add(contrato);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
            }

            return lista.OrderBy(m => m.Data).ToList();
        }
        
        /// <summary>
        /// Listar Simuladores Ativos
        /// </summary>
        /// <param name="dtInicio">dt Inicio</param>
        /// <param name="dtFim">dt Fim</param>
        /// <param name="idEstado">id Estado</param>
        /// <returns></returns>
        public List<SimuladoresAtivos> ListarSimuladoresAtivos(string dtInicio,string dtFim, string idEstado)
        {

            var lista = new List<SimuladoresAtivos>();

            if (string.IsNullOrEmpty(dtInicio))
                return lista;

            var dataInicial = Convert.ToDateTime(dtInicio);
            var dataFinal = (dtFim == "") ? Convert.ToDateTime(dtInicio) : Convert.ToDateTime(dtFim);

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProIntegracao"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("SpRelSimuladorAtivo", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DataInicio", dataInicial);
                    command.Parameters.AddWithValue("@DataFim", dataFinal);
                    command.Parameters.AddWithValue("@UF", (!string.IsNullOrEmpty(idEstado)) ? idEstado : null);


                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var simulador = new SimuladoresAtivos()
                        {
                            Estado          = dr["UF"].ToString()
                            , RazaoSocial   = dr["RazaoSocial"].ToString()
                            , CNPJ          = dr["CNPJ"].ToString()
                            , SMC           = dr["CodigoSMC"].ToString()
                        };
                        
                        lista.Add(simulador);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
            }

            return lista.OrderBy(m => m.Data).ToList();
        }

        /// <summary>
        /// Listar Aulas Simulador Ativo
        /// </summary>
        /// <param name="dtInicio">dt Inicio</param>
        /// <param name="dtFim">dt Fim</param>
        /// <param name="idEstado">Id Estado</param>
        /// <returns></returns>
        public List<AulasPorSimulador> ListarAulaSimuladorAtivo(string dtInicio, string dtFim, string idEstado)
        {

            var lista = new List<AulasPorSimulador>();

            if (string.IsNullOrEmpty(dtInicio))
                return lista;

            var dataInicial     = Convert.ToDateTime(dtInicio);
            var dataFinal       = (dtFim == "") ? Convert.ToDateTime(dtInicio) : Convert.ToDateTime(dtFim);

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProIntegracao"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("SpRelMediaAula", conn);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@DataInicio", dataInicial);
                    command.Parameters.AddWithValue("@DataFim", dataFinal);
                    command.Parameters.AddWithValue("@UF", (!string.IsNullOrEmpty(idEstado)) ? idEstado : null);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var aulasporsimulador = new AulasPorSimulador()
                        {
                            Mes = dr["Mês"].ToString()
                            , Estado = dr["UF"].ToString()
                            , NrMedioAulas = Convert.ToInt32(dr["Aulas"].ToString())
                            , NrSimuladores = Convert.ToInt32(dr["Simuladores"].ToString())
                            , Media = Convert.ToDecimal(dr["Média"].ToString())

                        };
    
                        lista.Add(aulasporsimulador);

                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;

            }

            return lista.OrderBy(m => m.Mes).ToList();
        }

    }
}
