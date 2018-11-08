using ProIntegracao.Data.EntidadeProSimulador;
using ProIntegracao.Model.DetranSpWS;
using ProIntegracao.Model.ReenvioAula;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioHistoricoAluno : BaseRepositorio<HistoricoAluno>
    {
        #region Propriedades
        private IList<string> EstadosPermitidosFinalizacaoManual
        {
            get
            {
                return new List<string>() { "AL", "AC", "AM", "BA", "GO", "PB", "PR", "RJ", "RS", "SP", "TO", "MS", "SC", "RN", "MG" };
            }
        }
        #endregion

        #region Métodos

        /// <summary>
        /// Listar Historico Aluno
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <param name="renach">Renach</param>
        /// <param name="nome">Nome</param>
        /// <returns></returns>
        public List<HistoricoAluno> ListarHistoricoAluno(string cpf, string renach, string nome, bool ativo)
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            List<HistoricoAluno> lista = new List<HistoricoAluno>();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    command = new SqlCommand("pr_selecionar_historico_aluno", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@CPF", cpf);
                    command.Parameters.AddWithValue("@RENACH", renach);
                    command.Parameters.AddWithValue("@NOME", nome);
                    command.Parameters.AddWithValue("@ATIVO", ativo);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var historico = new HistoricoAluno()
                        {
                            Nome = dr["NOME"].ToString()
                            , DataNascimento = Convert.ToDateTime(dr["DATA_NASCIMENTO"].ToString())
                            , CPF = dr["CPF"].ToString()
                            , Renach = dr["RENACH"].ToString()
                            , Curso = dr["CURSO"].ToString()
                            , Biometria = dr["BIOMETRIA"].ToString()
                            , CEP = dr["CEP"].ToString()
                            , Endereco = dr["ENDERECO"].ToString()
                            , Bairro = dr["BAIRRO"].ToString()
                            , Cidade = dr["CIDADE"].ToString()
                            , Estado = dr["ESTADO"].ToString()
                            , Telefone = dr["TELEFONE"].ToString()
                            , Celular = dr["CELULAR"].ToString()
                            , Status = dr["STATUS"].ToString()
                            , IdAluno = dr["ID_ALUNO"].ToString()
                        };

                        lista.Add(historico);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO HISTORICO ALUNO", ex.Message);
            }

            return lista;
        }

        /// <summary> RetornaAluno
        /// </summary>
        /// <param name="idAluno"></param>
        /// <returns></returns>
        public HistoricoAluno RetornaAluno(int idAluno)
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;

            IDataReader dr = null;

            var aluno = new HistoricoAluno();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    command = new SqlCommand("pr_selecionar_historico_aluno_individual", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_ALUNO", idAluno);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var historico = new HistoricoAluno()
                        {
                            Nome                = dr["NOME"].ToString()
                            , DataNascimento    = Convert.ToDateTime(dr["DATA_NASCIMENTO"].ToString())
                            , CPF               = dr["CPF"].ToString()
                            , Renach            = dr["RENACH"].ToString()
                            , Curso             = dr["CURSO"].ToString()
                            , Biometria         = dr["BIOMETRIA"].ToString()
                            , CEP               = dr["CEP"].ToString()
                            , Endereco          = dr["ENDERECO"].ToString()
                            , Bairro            = dr["BAIRRO"].ToString()
                            , Cidade            = dr["CIDADE"].ToString()
                            , Estado            = dr["ESTADO"].ToString()
                            , Telefone          = dr["TELEFONE"].ToString()
                            , Celular           = dr["CELULAR"].ToString()
                            , Status            = dr["STATUS"].ToString()
                            , IdAluno           = dr["ID_ALUNO"].ToString()
                        };

                        aluno = historico;
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO HISTORICO ALUNO", ex.Message);
            }

            return aluno;
        }

        /// <summary>Listar Matriculas
        /// </summary>
        /// <param name="idAluno"></param>
        /// <returns></returns>
        public List<Matricula> ListarMatriculas(int idAluno)
        {
            var lista = new List<Matricula>();

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("pr_selecionar_matriculas_historico_aluno", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@IDALUNO", idAluno);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var matricula = new Matricula()
                        {
                            IdMatricula = Convert.ToInt32(dr["ID_MATRICULA"].ToString())
                            ,
                            CodigoMatricula = dr["CODIGO_MATRICULA"].ToString()
                            ,
                            QtdAulas = Convert.ToInt32(dr["QTD_AULA"].ToString())
                            ,
                            DataMatricula = Convert.ToDateTime(dr["DATA"].ToString())
                            ,
                            Usuario = dr["USUARIO"].ToString()
                            ,
                            IdEmpresa = dr["ID_EMPRESA"].ToString()
                            ,
                            CodigoCFC = dr["CODIGO_CFC"].ToString()
                            ,
                            CFCOrigem = dr["CFC_ORIGEM"].ToString()
                            ,
                            CNPJ = dr["CNPJ"].ToString()
                            ,
                            Status = dr["STATUS_MATRICULA"].ToString()
                        };
                        lista.Add(matricula);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO HISTORICO ALUNO", ex.Message);
            }
            return lista;
        }

        /// <summary>Listar Agendas
        /// </summary>
        /// <param name="idMatricula"></param>
        /// <returns></returns>
        public List<Agenda> ListarAgendas(int idMatricula)
        {
            var lista = new List<Agenda>();

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    command = new SqlCommand("pr_selecionar_agendas_historico_aluno", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_MATRICULA", idMatricula);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var agenda = new Agenda()
                        {
                            IdMatricula = dr["ID_MATRICULA"].ToString()
                            , IdAgenda = dr["ID_AGENDA"].ToString()
                            , Sequencia = dr["SEQUENCIA"].ToString()
                            , HorarioAula = Convert.ToDateTime(dr["HORARIO_INICIO"].ToString())
                            , PSA = dr["PSA"].ToString()
                            , SMC = dr["SMC"].ToString()
                            , CPFInstrutor = dr["CPF_INSTRUTOR"].ToString()
                            , NomeInstrutor = dr["NOME_INSTRUTOR"].ToString()
                            , ModeloAula = dr["MODELO"].ToString()
                            , Status = dr["STATUS"].ToString()
                        };
                        lista.Add(agenda);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO HISTORICO ALUNO", ex.Message);
            }

            return lista;

        }

        /// <summary>Listar Historico
        /// </summary>
        /// <param name="idMatricula">Id Agenda</param>
        /// <returns></returns>
        public List<HistoricoAula> ListarHistorico(int idMatricula)
        {
            var lista = new List<HistoricoAula>();

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("pr_selecionar_aulas_historico_aluno", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_MATRICULA", idMatricula);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var historico = new HistoricoAula()
                        {
                            IdAula                  = dr["ID_AULA"].ToString()
                            , IdAgenda              = dr["ID_AGENDA"].ToString()
                            , HorarioInicio         = Convert.ToDateTime(dr["HORARIO_INICIO"].ToString())
                            , Curso                 = dr["CURSO"].ToString()
                            , SessionId             = dr["SESSION_ID"].ToString()
                            , IdStatus              = Convert.ToInt32(dr["ID_AULA_STATUS"].ToString())
                            , Status                = dr["STATUS"].ToString()
                            , ModelodeAula          = dr["MODELO"].ToString()
                            , IdModelo              = Convert.ToInt32(dr["ID_MODELO"].ToString())
                            , Estado                = dr["UF"].ToString() 
                            , PodeReenviar          = VerificaPodeReenviar(dr["UF"].ToString(), dr["ID_AULA"].ToString(), dr["CURSO"].ToString(), dr["STATUS"].ToString(), dr["HORARIO_FIM"].ToString())
                            , PodeCancelar          = VerificaPodeCancelar(dr["UF"].ToString(), dr["ID_AULA"].ToString(), dr["CURSO"].ToString(), dr["STATUS"].ToString())
                            , PodeConsultar         = VerificaPodeConsultar(dr["UF"].ToString(), dr["ID_AULA"].ToString(), dr["CURSO"].ToString(), dr["STATUS"].ToString())
                            , ConsultarAulaDetran   = VerificarConsultaAulaDetran(dr["STATUS"].ToString(),dr["CURSO"].ToString(), dr["UF"].ToString())
                            , IdSimulador           = dr["ID_SIMULADOR"].ToString()
                            , PodeAtualizar         = VerificaPodeAtualizar(dr["UF"].ToString(), dr["STATUS"].ToString())
                            , Info                  = dr["INFO"].ToString()
                            , CPF                   = dr["CPF"].ToString()
                            , IdentificadorAula     = Convert.ToInt64(dr["IDENTIFICADORAULA"].ToString())
                            , IdMatricula           = dr["ID_MATRICULA"].ToString()
                        };

                        if (dr["HORARIO_FIM"].ToString() != "")
                            historico.HorarioFim = Convert.ToDateTime(dr["HORARIO_FIM"].ToString());
                        else
                            historico.HorarioFim = (DateTime?)null;

                        lista.Add(historico);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO HISTORICO ALUNO", ex.Message);
            }
            return lista;
        }

        /// <summary>Verificar Consulta Aula Detran
        /// </summary>
        /// <param name="status">Status</param>
        /// <param name="curso">Curso</param>
        /// <param name="estado">Estado</param>
        /// <returns></returns>
        private bool VerificarConsultaAulaDetran(string status, string curso, string estado)
        {
            var result = false;

            switch (status.ToUpper())
            {
                case "REALIZADA":
                case "CANCELADA":
                case "RECUSADA DETRAN":
                    if (curso.ToUpper().Trim() == "B" && estado.ToUpper() == "SP")
                        result = true;
                    break;
            }

            return result;
        }
        
        /// <summary>Verificar Pode Atualizar
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="status">Status</param>
        /// <returns></returns>
        private bool VerificaPodeAtualizar(string estado, string status)
        {
            var result = false;

            switch (status.ToUpper())
            {
                case "REALIZADA":
                case "PENDENTE":
                case "CANCELADA":
                case "CANCELADA USUÁRIO":
                case "RECUSADA DETRAN":
                    result = true;
                    break;
                
                
            }

            return result;
        }
        
        /// <summary> Verifica se Pode Reenviar
        /// 
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="idAula">idAula</param>
        /// <param name="status">Status</param>
        /// <returns></returns>
        private bool VerificaPodeConsultar(string estado, string idAula, string curso, string status)
        {
            var result = false;
            
            switch (status.ToUpper())
            {
                case "REALIZADA":
                    result = true;
                    break;
                case "RECUSADA DETRAN":
                case "CANCELADA":
                case "CANCELADA USUÁRIO":
                    result = false;
                    break;
                default:
                    break;
            }

            return result;
        }
        
        /// <summary> Verifica se Pode Reenviar
        /// 
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="idAula">idAula</param>
        /// <param name="status">Status</param>
        /// <returns></returns>
        private bool VerificaPodeReenviar(string estado, string idAula, string curso, string status, string horario_fim)
        {
            var result = false;
            if (PermiteEstadoFinalizarManual(estado))
            {
                switch (status.ToUpper())
                {
                    case "EM ANDAMENTO":
                    case "RECUSADA DETRAN":
                    case "PENDENTE":
                        if (curso.ToUpper().Trim().Equals("B") && estado.ToUpper().Equals("SP"))
                            result = true;
                        break;
               }
            }

            return result;
        }

        /// <summary>Verifica se Pode Reenviar
        /// 
        /// </summary>
        /// <param name="estado">Estado</param>
        /// <param name="idAula">idAula</param>
        /// <param name="status">Status</param>
        /// <returns></returns>
        private bool VerificaPodeCancelar(string estado, string idAula, string curso, string status)
        {
            var result = false;

            if (PermiteEstadoFinalizarManual(estado))
            {
                switch (status.ToUpper())
                {
                    case "REALIZADA":
                    case "PENDENTE":
                    case "RECUSADA DETRAN":
                    case "EM ANDAMANTO":
                        if (curso.ToUpper().Trim().Equals("B") && estado.ToUpper().Trim().Equals("SP"))
                            result = true;
                        else if (estado.ToUpper().Trim().Equals("RS"))
                            result = true;
                        break;
                    case "CANCELADA USUÁRIO":
                    case "CANCELADA":
                        result = false;
                        break;
                } 
                
            }
            
            return result;
        }

        /// <summary>Retorna Detalhe Aluno
        /// </summary>
        /// <param name="idAluno">ID Aluno</param>
        /// <param name="idAula">Id Aula</param>
        /// <returns></returns>
        public Aluno RetornaDetalheAluno(int idAluno, int idAula)
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            var al = new Aluno();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("pr_selecionar_historico_aluno_detalhe_aula", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_ALUNO", idAluno);
                    command.Parameters.AddWithValue("@ID_AULA", idAula);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var aluno = new Aluno()
                        {
                            CFC = dr["CODIGO_CFC"].ToString()
                            ,
                            Nome = dr["NOME"].ToString()
                            ,
                            Renach = dr["RENACH"].ToString()
                            ,
                            Instrutor = dr["NOME_INSTRUTOR"].ToString()
                            ,
                            CPFInstrutor = dr["CPF_INSTRUTOR"].ToString()
                            ,
                            Modelo = dr["MODELO"].ToString()
                            ,
                            Session_ID = dr["SESSION_ID"].ToString()
                            ,
                            VelocidadeMedia = dr["VELOCIDADE_MEDIA"].ToString()
                            ,
                            TempoTrajeto = dr["TEMPO_TRAJETO"].ToString()
                            ,
                            HorarioInicio = Convert.ToDateTime(dr["HORARIO_INICIO"].ToString())
                            ,
                            HorarioFinal = Convert.ToDateTime(dr["HORARIO_FIM"].ToString())
                        };

                        al = aluno;
                    }

                    if (dr.NextResult())
                    {
                        var listaErro = new List<Erro>();

                        while (dr.Read())
                        {
                            var erro = new Erro()
                            {
                                DescricaoErro = dr["DESCRICAO"].ToString()
                                , Quantidade = Convert.ToInt32(dr["QTD_INFRACAO"].ToString())
                                , TipoErro = Convert.ToInt32(dr["ERRO"].ToString())
                            };

                            listaErro.Add(erro);
                        }

                        al.Erros = listaErro;
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO HISTORICO ALUNO", ex.Message);
            }

            return al;
        }
        
        /// <summary> Retorna Lista Modelo Aula
        /// </summary>
        /// <returns></returns>
        public List<Modelo> RetornaListaModeloAula(int idSimulador)
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            var lista = new List<Modelo>();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {

                    var query = new StringBuilder();

                    query.Append("SELECT M.ID_MODELO, M.NOME");
                    query.Append(" FROM TB_SIMULADOR_AULA_ATIVA SA WITH(NOLOCK)");
                    query.Append(" INNER JOIN TB_MODELO M(NOLOCK) ON SA.ID_MODELO_AULA = M.ID_MODELO");
                    query.Append(" WHERE M.VIGENTE = 1");
                    query.AppendFormat(" AND SA.ID_SIMULADOR = {0}", idSimulador);
                    query.Append(" ORDER BY M.ID_MODELO");
                    
                    conn.Open();
                    command = new SqlCommand(query.ToString(), conn)
                    {
                        CommandType = CommandType.Text
                    };
                    dr = command.ExecuteReader();
                    while (dr.Read())
                    {
                        var modelo = new Modelo()
                        {
                            ID_MODELO = Convert.ToInt32(dr["ID_MODELO"].ToString())
                            , NOME = dr["NOME"].ToString()

                        };

                        lista.Add(modelo);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO MODELO AULA", ex.Message);
            }

            return lista;
        }

        /// <summary>Inserir Aula Reenvio
        /// </summary>
        /// <param name="idAula">Id Aula</param>
        /// <returns>True/False</returns>
        public bool InserirAulaReenvio(int idAula)
        {

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;

            var result = false;

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    command = new SqlCommand("pr_inserir_aula_reenvio", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    command.Parameters.AddWithValue("@ID_AULA", idAula);
                    command.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO INSERIR AULA REENVIO", ex.Message);
            }
            return result;
        }
        
        /// <summary>Permite Estado Finalizar Manual
        /// </summary>
        /// <param name="uf">Estado</param>
        /// <returns></returns>
        private bool PermiteEstadoFinalizarManual(string uf)
        {
            return EstadosPermitidosFinalizacaoManual.Contains(uf);
        }
        
        /// <summary>Enviar Aula
        /// 
        /// </summary>
        /// <param name="idAgenda">Id Agenda</param>
        /// <param name="idAula">Id Aula</param>
        /// <param name="idAluno">Id Aluno</param>
        /// <returns></returns>
        public bool EnviarAula(int idAgenda)
        {
            var result = false;
            try
            {
                using (ReenvioAulaAutomaticoClient servico = new ReenvioAulaAutomaticoClient())
                {
                    bool retornoservico = servico.EnvioAulaIntegracao(Convert.ToInt32(idAgenda));

                    //Caso não foi enviado para Serviço, deixar na fila para reenvio
                    if (!retornoservico)
                    {
                        var resultado = AtualizarAgenda(Convert.ToInt32(idAgenda));
                    }
                };
            }
            catch (Exception ex)
            {
                var message = string.Format("Erro ao enviar AULA - ID AGENDA : {0} - Mensagem : {1}", idAgenda, ex.Message);
                //InserirLog("REPOSITORIO HISTORICO ALUNO - ENVIAR AULA", message);
            }

            return result;
        }

        /// <summary>Cancelar Aula
        /// 
        /// </summary>
        /// <param name="idAgenda">Id Agenda</param>
        /// <param name="idAula">Id Aula</param>
        /// <param name="idAluno">Id Aluno</param>
        /// <returns></returns>
        public bool CancelarAula(int idAgenda)
        {
            var result = false;
            try
            {

                using (ReenvioAulaAutomaticoClient servico = new ReenvioAulaAutomaticoClient())
                {
                    bool retornoservico = servico.CancelarAulaIntegracao(Convert.ToInt32(idAgenda));

                    //Caso não foi enviado para Serviço, deixar na fila para reenvio
                    if (!retornoservico)
                    {
                        var resultado = AtualizarAgenda(Convert.ToInt32(idAgenda));
                    }
                };
            }
            catch (Exception ex)
            {
                var message = string.Format("Erro ao enviar AULA - ID AGENDA : {0} - Mensagem {1}: ", idAgenda, ex.Message);
                //InserirLog("REPOSITORIO HISTORICO ALUNO - ENVIAR AULA", message);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idAgenda"></param>
        /// <returns></returns>
        private Agenda RetornaAgenda(int idAgenda)
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            var agenda = new Agenda();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = string.Format("SELECT TOP 1 * FROM TB_AGENDA A WITH (NOLOCK) WHERE ID_AGENDA = {0}", idAgenda);

                    command = new SqlCommand(query, conn)
                    {
                        CommandType = CommandType.Text
                    };
                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var ag = new Agenda()
                        {
                            IdAluno = Convert.ToInt32(dr["ID_ALUNO"].ToString())
                            , IdInstrutor = Convert.ToInt32(dr["ID_INSTRUTOR"].ToString())
                        };
                        agenda = ag;
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO HISTORICO ALUNO - RECUPERA AGENDA PARA ENVIO DE AULA", ex.Message);
            }

            return agenda;
        }

        /// <summary>Alterar Status Modelo
        /// </summary>
        /// <param name="idAula">Id Aula</param>
        /// <param name="idStatus">Id Status</param>
        /// <param name="idModelo">Id Modelo</param>
        /// <returns></returns>
        public bool AlterarStatusModeloAula(int idAula, int idStatus, int idModelo)
        {
            var result = false;

            //Atualizar Status

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;


            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    command = new SqlCommand("pr_atualizar_status_modelo_aula", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    command.Parameters.AddWithValue("@ID_AULA", idAula);
                    command.Parameters.AddWithValue("@ID_STATUS", idStatus);
                    command.Parameters.AddWithValue("@ID_MODELO", idModelo);

                    command.ExecuteNonQuery();

                    result = true;

                }
            }
            catch (Exception ex)
            {
                var message = string.Format("Alterar Status, Modelo Aula | ID AULA : {0} - ID_STATUS : {1} - ID_MODELO : {2} | Message : {3}", idAula, idStatus, idModelo, ex.Message);
                //InserirLog("REPOSITORIO HISTORICO ALUNO - Alterar Status Modelo", message);                    
            }

            return result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="identificadorAula"></param>
        /// <returns></returns>
        public ConsultaAulaDetran ConsultaDetran(string cpf , long identificadorAula)
        {
            using (SimuladorServiceClient service = new SimuladorServiceClient())
            {
                var retorno = new ConsultaAulaDetran();

                var retornoDetran = service.consultarAula(cpf, identificadorAula);

                if (retornoDetran != null && retornoDetran.cpfAluno > 0)
                {
                    retorno.cpf = retornoDetran.cpfAluno.ToString() != string.Empty ? retornoDetran.cpfAluno.ToString() : string.Empty;
                    retorno.Status = retornoDetran.statusAula.ToString() != string.Empty ? retornoDetran.statusAula.ToString() : string.Empty;

                    if (retorno.Status.ToUpper() == "Cancelada".ToUpper())
                    {
                        retorno.ResponsavelPeloCancelamento = retornoDetran.cpfCancelamentoAula.ToString() != string.Empty ? retornoDetran.cpfCancelamentoAula.ToString() : string.Empty;
                    }
                }
                else
                {
                    retorno.MensagemRetorno = "A consulta não obteve resultado no Detran.";
                }
                return retorno;
            }
           
        }
        
        /// <summary>
        /// Retornar Finalizar Aula
        /// </summary>
        /// <param name="idMatricula">Id Matricula</param>
        /// <returns></returns>
        public List<FinalizarAula> RetornarFinalizarAula(int idMatricula)
        {

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            var lista = new List<FinalizarAula>();
            //var aula = new FinalizarAula();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {

                    var query = new StringBuilder();

                    //query.Append(" SELECT DISTINCT");
                    //query.Append("	P.CPF");
                    //query.Append("	, A.ID_AULA");
                    //query.Append("	, UPPER(P.NOME) NOME");
                    //query.Append("	, UF.SIGLA UF");
                    //query.Append("	, AG.ID_CFC");
                    //query.Append("	, E.NOME_FANTASIA");
                    //query.Append("	, MD.ID_MODELO");
                    //query.Append("	, MD.NOME [MODELO]");
                    //query.Append("	, I.DESCRICAO");
                    //query.Append("	, MA.ID_MATRICULA");
                    //query.Append("	, ROUND((3000 - 2000 * RAND()), 0)	[DISTANCIA]");
                    //query.Append("	, ROUND((30 - 20 * RAND()), 0)		[TEMPOCIRCULACAO]");
                    //query.Append("	, ROUND((100 - 50 * RAND()), 0)		[VELOCIDADEMAXIMA]");
                    //query.Append("	, ROUND((15 - 8 * RAND()), 0)		[VELOCIDADEMEDIA]");
                    //query.Append("	, (SELECT ABS(CHECKSUM(NEWID())) % 15 + 1)	[EVENTO]");
                    //query.Append(" FROM  TB_MATRICULA MT                    WITH(NOLOCK)");
                    //query.Append("	INNER JOIN TB_AGENDA AG					WITH (NOLOCK)	ON MT.ID_MATRICULA = AG.ID_MATRICULA ");
                    //query.Append("	INNER JOIN TB_AULA A					WITH (NOLOCK)	ON AG.ID_AGENDA = A.ID_CLIENTE_AGENDA ");
                    //query.Append("	INNER JOIN TB_PESSOA P					WITH (NOLOCK)	ON A.ID_ALUNO = P.ID_PESSOA");
                    //query.Append("	INNER JOIN TB_EMPRESA E					WITH (NOLOCK)	ON AG.ID_CFC = E.ID_EMPRESA");
                    //query.Append("	INNER JOIN TB_ENDERECO ED				WITH (NOLOCK)	ON P.ID_ENDERECO = ED.ID_ENDERECO");
                    //query.Append("	INNER JOIN TB_MUNICIPIO M				WITH (NOLOCK)	ON ED.ID_MUNICIPIO = M.ID_MUNICIPIO");
                    //query.Append("	INNER JOIN TB_UF UF						WITH (NOLOCK)	ON M.ID_UF = UF.ID_UF");
                    //query.Append("	LEFT JOIN TB_MATRICULA_MODELO	MA		WITH (NOLOCK)	ON MA.ID_MATRICULA	= MT.ID_MATRICULA");
                    //query.Append("	LEFT JOIN TB_AULA_MODELO AM				WITH (NOLOCK)	ON MA.ID_MODELO		= AM.ID_MODELO");
                    //query.Append("	LEFT JOIN TB_MODELO MD					WITH (NOLOCK)	ON AM.ID_MODELO		= MD.ID_MODELO");
                    //query.Append("	LEFT JOIN TB_RESULTADO R				WITH (NOLOCK)	ON A.ID_AULA = R.ID_AULA");
                    //query.Append("	LEFT JOIN TB_INFRACOES I				WITH (NOLOCK)   ON R.CODIGO_INFRACAO = I.CODIGO");
                    //query.Append(" WHERE ");
                    //query.AppendFormat("	AG.ID_MATRICULA = {0}", idMatricula);
                    //query.Append(" AND");
                    //query.Append("	NOT MA.ID_MATRICULA IS NULL");
                    //query.Append(" AND");
                    //query.Append("	NOT MD.NOME IS NULL");
                    //query.Append(" ORDER BY A.ID_AULA");


                    query.Append(" SELECT DISTINCT ");
                    query.Append("	P.CPF	 ");
                    query.Append("	, A.ID_AULA	 ");
                    query.Append("	, AG.ID_AGENDA ");
                    query.Append("	, UPPER(P.NOME) NOME ");
                    query.Append("	, UF.SIGLA	UF ");
                    query.Append("	, AG.ID_CFC	 ");
                    query.Append("	, E.NOME_FANTASIA ");
                    query.Append("	, AG.EXERCICE_BLOCK ");
                    query.Append("	, A.ID_AULA_STATUS ");
                    query.Append("	, A.HORARIO_FIM ");
                    query.Append("	, MT.ID_MATRICULA");
                    query.Append("	, AG.EXERCICE_BLOCK ID_MODELO ");
                    query.Append("	, (SELECT MD.NOME FROM TB_MODELO MD WHERE ID_MODELO = AG.EXERCICE_BLOCK) MODELO ");
                    query.Append("	FROM  TB_MATRICULA MT	");
                    query.Append("	INNER JOIN TB_AGENDA AG					WITH (NOLOCK)	ON MT.ID_MATRICULA = AG.ID_MATRICULA  ");
                    query.Append("	INNER JOIN TB_AULA A					WITH (NOLOCK)	ON AG.ID_AGENDA = A.ID_CLIENTE_AGENDA  ");
                    query.Append("	INNER JOIN TB_PESSOA P					WITH (NOLOCK)	ON A.ID_ALUNO = P.ID_PESSOA	 ");
                    query.Append("	INNER JOIN TB_EMPRESA E					WITH (NOLOCK)	ON AG.ID_CFC = E.ID_EMPRESA	 ");
                    query.Append("	INNER JOIN TB_ENDERECO ED				WITH (NOLOCK)	ON P.ID_ENDERECO = ED.ID_ENDERECO	 ");
                    query.Append("	INNER JOIN TB_MUNICIPIO M				WITH (NOLOCK)	ON ED.ID_MUNICIPIO = M.ID_MUNICIPIO	 ");
                    query.Append("	INNER JOIN TB_UF UF						WITH (NOLOCK)	ON M.ID_UF = UF.ID_UF	 ");
                    query.Append("	WHERE 	 ");
                    query.AppendFormat("	AG.ID_MATRICULA = {0}", idMatricula);
                    query.Append("	AND  ");
                    query.Append("		HORARIO_FIM IS NULL ");
                    query.Append("	ORDER BY A.ID_AULA ");

                    conn.Open();
                    command = new SqlCommand(query.ToString(), conn)
                    {
                        CommandType = CommandType.Text
                    };
                    dr = command.ExecuteReader();

                   

                    while (dr.Read())
                    {
                        var aula = new FinalizarAula()
                        {
                            CPF                     = dr["CPF"].ToString()
                           , NOME                   = dr["NOME"].ToString()
                           , ID_MATRICULA           = dr["ID_MATRICULA"].ToString()
                           , ESTADO                 = dr["UF"].ToString()
                           , ID_CFC                 = dr["ID_CFC"].ToString()
                           , NOME_FANTASIA          = dr["NOME_FANTASIA"].ToString()
                           , MODELO                 = dr["MODELO"].ToString()
                           , ID_MODELO              = dr["ID_MODELO"].ToString()
                           , ID_AULA                = dr["ID_AULA"].ToString()
                           
                        };
                        
                        lista.Add(aula);
                    }

                    

                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO MODELO AULA", ex.Message);
            }

            //aula.Eventos = lista;

            return lista;

        }
        
        /// <summary>
        /// Listar Evento
        /// </summary>
        /// <param name="idModelo">Id Modelo</param>
        /// <returns></returns>
        public List<EventosFinalizacao> ListarEventos(int idModelo)
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;

            var lista = new List<EventosFinalizacao>();


            try
            {
                using (conn = new SqlConnection(connectionString))
                {

                    var query = new StringBuilder();

                    query.Append(" SELECT ");
                    query.Append(" 	EA.CodigoEvento [CODIGOEVENTO]");
                    query.Append(" 	, EA.TipoEvento [TIPOEVENTO]");
                    query.Append(" 	, EA.DescricaoEvento [DESCRICAO]");
                    query.Append(" 	,ROUND((3000 - 2000 * RAND()), 0)[DISTANCIA]");
                    query.Append(" 	,ROUND((30 - 20 * RAND()), 0)[TEMPOCIRCULACAO]");
                    query.Append(" 	,ROUND((100 - 50 * RAND()), 0)[VELOCIDADEMAXIMA]");
                    query.Append(" 	,ROUND((15 - 8 * RAND()), 0)[VELOCIDADEMEDIA]");
                    query.Append(" 	,(SELECT ABS(CHECKSUM(NEWID())) % 15 + 1)	[EVENTO]");
                    query.Append(" FROM TB_EVENTOS_POR_AULA EA");
                    query.AppendFormat(" WHERE Aula{0} = 1", idModelo);
                    
                    conn.Open();
                    command = new SqlCommand(query.ToString(), conn)
                    {
                        CommandType = CommandType.Text
                    };
                    dr = command.ExecuteReader();
                    
                    while (dr.Read())
                    {
                        var evento = new EventosFinalizacao()
                        {
                             DESCRICAO = dr["DESCRICAO"].ToString()
                            , DISTANCIA = dr["DISTANCIA"].ToString()
                            , TEMPOCIRCULACAO = dr["TEMPOCIRCULACAO"].ToString()
                            , VELOCIDADEMAXIMA = dr["VELOCIDADEMAXIMA"].ToString()
                            , VELOCIDADEMEDIA = dr["VELOCIDADEMEDIA"].ToString()
                            , EVENTO = dr["EVENTO"].ToString()
                            , ID_EVENTO = dr["CODIGOEVENTO"].ToString()
                            , TIPO_EVENTO = dr["TIPOEVENTO"].ToString()
                            , ID_MODELO = idModelo.ToString()
                        };

                        lista.Add(evento);
                    }
                }
            }
            catch (Exception ex)
            {
                var msgErro = ex.Message;
                //InserirLog("REPOSITORIO HISTORICO AULA - EVENTO FINALIZACAO", ex.Message);
            }
            return lista;
        }

        /// <summary>
        /// Atualizar Finalizacao Aula
        /// </summary>
        /// <param name="aula">Historico de Aula</param>
        /// <returns></returns>
        public bool AtualizarFinalizacaodeAula(List<FinalizarAula> lista)
        {

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            var result = false;
            
            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                command = new SqlCommand("pr_atualiza_finalizacao_aula", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                SqlTransaction transaction;
                transaction = conn.BeginTransaction("SampleTransaction");
                command.Transaction = transaction;

                try
                {
                    foreach (var item in lista)
                    {
                        var totalInfracao = 0;
                        var totalerros = 0;

                        foreach (var evento in item.Eventos)
                        {
                            command.Parameters.AddWithValue("@ID_AULA", Convert.ToInt32(item.ID_AULA));
                            command.Parameters.AddWithValue("@ID_EVENTO", Convert.ToInt32(evento.ID_EVENTO));
                            //command.Parameters.AddWithValue("@QTD_INFRACOES", Convert.ToInt32(evento.QTD_INFRACOES));
                            command.Parameters.AddWithValue("@TIPO_EVENTO", Convert.ToInt32(evento.TIPO_EVENTO));
                            command.Parameters.AddWithValue("@DISTANCIA_PERCORRIDA", Convert.ToInt32(evento.DISTANCIA));
                            command.Parameters.AddWithValue("@VELOCIDADE_MAXIMA", Convert.ToInt32(evento.VELOCIDADEMAXIMA));
                            command.Parameters.AddWithValue("@VELOCIDADE_MEDIA", Convert.ToInt32(evento.VELOCIDADEMEDIA));
                            command.Parameters.AddWithValue("@TEMPO_TRAJETO", Convert.ToInt32(evento.TEMPOCIRCULACAO));
                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                            result = true;

                            //if (evento.TIPO_EVENTO == "1") totalInfracao += Convert.ToInt32(evento.QTD_INFRACOES);
                            //if (evento.TIPO_EVENTO == "2" || evento.TIPO_EVENTO == "3") totalerros += Convert.ToInt32(evento.QTD_INFRACOES);

                        }

                        // Atualizar tabela com Total de Infracoes e Erros
                        string query = string.Format("UPDATE TB_AULA SET TOTAL_INFRACOES = {0}, TOTAL_ERROS = {1} WHERE ID_AULA = {2}", totalInfracao, totalerros, item.ID_AULA);
                        command = new SqlCommand(query, conn)
                        {
                            CommandType = CommandType.Text,
                            Transaction = transaction
                        };
                        command.ExecuteNonQuery();
                        
                        //Limpa Totalinfracoes e erros
                        totalInfracao = 0;
                        totalerros = 0;

                    }

                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch 
                    {
                        var msgErro = ex.Message;
                        //InserirLog("RPOSITORIO HITORICO ALUNO", string.Format("Rollback Exception Type: {0} | Message : {1}" + ex2.GetType(), ex2.Message));
                    }
                    result = false;
                }
            }
            return result;
        }


        public bool AtualizarAgenda(int idAgenda)
        {

            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            var result = false;

            using (conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UDPATE TB_AGENDA SET REENVIO = 0  WHERE ID_AGENDA = " + idAgenda;
                command = new SqlCommand(query, conn)
                {
                    CommandType = CommandType.Text
                };

                SqlTransaction transaction;
                transaction = conn.BeginTransaction("SampleTransaction");
                command.Transaction = transaction;

                try
                {
                    command.ExecuteNonQuery();
                    result = true;
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch
                    {
                        var msgErro = ex.Message;
                        //InserirLog("RPOSITORIO HITORICO ALUNO", string.Format("Rollback Exception Type: {0} | Message : {1}" + ex2.GetType(), ex2.Message));
                    }
                    result = false;
                }
            }
            return result;
        }


        #endregion
    }
}
