using ProIntegracao.Data.Entidade;
using ProIntegracao.Model.Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace ProIntegracao.Model.Repositorio
{
    public class RepositorioSituacaoAula : BaseRepositorioProSimulador
    {
        /// <summary>
        /// Listar Situacao Aula 
        /// </summary>
        /// <param name="cpf">Cpf</param>
        /// <param name="idEstado">Id Estado</param>
        /// <param name="dtInicio">Dt Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="renach">Renach</param>
        /// <param name="status">Status</param>
        /// <param name="curso">Curso</param>
        /// <returns></returns>
        public List<SituacaoAula> ListarSituacaoAula(string cpf
            , int idEstado
            , string renach
            , string dtInicio
            , string dtFim
            , int status
            , int curso)
        {
            string connectionString = TecnoStr.ConnTec.decryptStr(ConfigurationManager.ConnectionStrings["ProSimulador"].ConnectionString);
            SqlConnection conn = null;
            SqlCommand command = null;
            IDataReader dr = null;
            
            List<SituacaoAula> lista = new List<SituacaoAula>();

            try
            {
                using (conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    command = new SqlCommand("pr_selecionar_situacao_aula", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CPF", cpf);
                    command.Parameters.AddWithValue("@IDESTADO", idEstado);
                    command.Parameters.AddWithValue("@RENACH", renach);
                    command.Parameters.AddWithValue("@DTINICIO", dtInicio);
                    command.Parameters.AddWithValue("@DTFIM", dtFim);
                    command.Parameters.AddWithValue("@IDSTATUS", status);
                    command.Parameters.AddWithValue("@IDCURSO", curso);

                    dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        var situacao = new SituacaoAula()
                        {
                            UF              = dr["UF"].ToString()
                            , IdAula        = Convert.ToInt32(dr["ID_AULA"].ToString())
                            , IdEmpresa     = Convert.ToInt32(dr["ID_EMPRESA"].ToString())
                            , CFC           = dr["CFC"].ToString()
                            , Municipo      = dr["MUNICIPIO"].ToString()
                            , CNPJ          = dr["CNPJ"].ToString()
                            , RazaoSocial   = dr["RAZAO_SOCIAL"].ToString()
                            , Cpf           = dr["CPF"].ToString()
                            , Nome          = dr["NOME"].ToString()
                            , DtNascimento  = Convert.ToDateTime(dr["DATA_NASCIMENTO"].ToString())
                            , Renach        = dr["RENACH"].ToString()
                            , DtAula        = Convert.ToDateTime(dr["DATA_AULA"].ToString())
                            , Agendado      = dr["HORA_AULA"].ToString()
                            , Inicio        = dr["HORARIO_INICIO"].ToString()
                            , Fim           = dr["HORARIO_FIM"].ToString()
                            , Status        = dr["STATUS"].ToString()
                            , Agenda        = (!string.IsNullOrEmpty(dr["AGENDA"].ToString()))
                                                ? Convert.ToInt32(dr["AGENDA"].ToString())
                                                : 0
                            , Sequencia     = dr["SEQUENCIA"].ToString()
                            , TipoCurso     = dr["TIPO_CURSO"].ToString()
                            , Modelo        = dr["MODELO"].ToString()
                            , PSA           = dr["PSA"].ToString()
                            , SESSION_ID    = dr["SESSION_ID"].ToString()
                        };

                        lista.Add(situacao);
                    }
                }
            }
            catch (Exception ex)
            {
              
            }

            return lista;
        }

      
        
    

        public string PrettyXml(string xml)
        {
            var stringBuilder = new StringBuilder();

            var element = XElement.Parse(xml);

            var settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;

            using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                element.Save(xmlWriter);
            }

            return stringBuilder.ToString();
        }
    }
}
