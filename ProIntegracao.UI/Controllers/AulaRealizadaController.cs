using ProIntegracao.Data.EntidadeProSimulador;
using ProIntegracao.Model.Repositorio;
using ProIntegracao.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProIntegracao.UI.Controllers
{
    /// <summary>
    /// Aula Realizada Controller
    /// </summary>
    public class AulaRealizadaController : BaseController
    {
        #region Variaveis

        private RepositorioAulaRealizada _repo = new RepositorioAulaRealizada();

        #endregion
        
        #region Action

        /// <summary>
        /// INDEX
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var model = new AulaRealizadaViewModel();
            ViewBag.Title = RetornaNomePagina("/AulaRealizada");
          
            return View(model);
        }

        /// <summary>
        /// Contratos Firmados
        /// </summary>
        /// <returns></returns>
        public ActionResult ContratosFirmados()
        {
            var model = new AulaRealizadaViewModel();
            ViewBag.Title = RetornaNomePagina("/AulaRealizada/ContratosFirmados");
           
            return View(model);
        }

        /// <summary>Simuladores Ativos
        /// </summary>
        /// <returns></returns>
        public ActionResult SimuladoresAtivos()
        {
            var model = new AulaRealizadaViewModel();
            ViewBag.Title = RetornaNomePagina("/AulaRealizada/SimuladoresAtivos");
            
            return View(model);
        }
        
        /// <summary>
        /// Aulas Por Simulador
        /// </summary>
        /// <returns></returns>
        public ActionResult AulasPorSimulador()
        {
            var model = new AulaRealizadaViewModel();
            ViewBag.Title = RetornaNomePagina("/AulaRealizada/AulasPorSimulador");

            
            return View(model);
        }


      
        
        #endregion
        
        #region Métodos



        /// <summary>
        /// Listar Aula Realizada
        /// </summary>
        /// <param name="dtInicio">DtInicio</param>
        /// <param name="dtFim">DtFim</param>
        /// <param name="idEstado">Id Estado</param>
      
        /// <returns></returns>
        public ActionResult ListarMetodos(string dtInicio, string dtFim, string idEstado)
        {
            var lista = _repo.ListarAulaRealizada(dtInicio, dtFim, idEstado);

            string message = string.Format("Filtro | Data Inicio : {0} - Data Fim : {1} - IdEstado : {2}", dtInicio, dtFim, idEstado);
       

            return PartialView("~/Views/AulaRealizada/_listarAulaRealizada.cshtml", lista);

        }


       


        /// <summary>
        /// Listar Contratos Firmados
        /// </summary>
        /// <param name="dtInicio">Data Inicio</param>
        /// <param name="idEstado">Id Estado</param>
        /// <returns></returns>
        public ActionResult ListarContratosFirmados(string dtInicio, string idEstado)
        {
            var lista = _repo.ListarContratosFirmados(dtInicio, idEstado);
            string message = string.Format("Filtro | Data Inicio : {0} - IdEstado : {1}", dtInicio,idEstado);
         
            return PartialView("~/Views/AulaRealizada/_listaContratos.cshtml", lista);
        }
        
       

        /// <summary>
        /// Listar Simuladores Ativos
        /// </summary>
        /// <param name="dtInicio">Data Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="idEstado">Id Estado</param>
        /// <returns></returns>
        public ActionResult ListarSimuladoresAtivos(string dtInicio, string dtFim, string idEstado)
        {
            var lista = _repo.ListarSimuladoresAtivos(dtInicio, dtFim, idEstado);
            string message = string.Format("Filtro | Data Inicio : {0} - - Data Fim : {1}  IdEstado : {2}", dtInicio, dtFim, idEstado);
            
            return PartialView("~/Views/AulaRealizada/_listaSimuladoresAtivos.cshtml", lista);
        }
        
       

        /// <summary>
        /// Listar Aulas POr Simulador
        /// </summary>
        /// <param name="dtInicio">Data Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="idEstado">Id do Estado</param>
        /// <returns></returns>
        public ActionResult ListarAulasPorSimulador(string dtInicio, string dtFim, string idEstado)
        {
            var lista = _repo.ListarAulaSimuladorAtivo(dtInicio, dtFim, idEstado);

            string message = string.Format("Filtro | Data Inicio : {0} - - Data Fim : {1}  IdEstado : {2}", dtInicio, dtFim, idEstado);
            

            return PartialView("~/Views/AulaRealizada/_listaAulasPorSimulador.cshtml", lista);
        }




        
        
        /// <summary>
        /// Converter Lista para DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataTable ConvertToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }



        #endregion
        
        #region Exportar

        /// <summary>
        ///  Listar Aulas Por Simulador JSON
        /// </summary>
        /// <param name="dtInicio">Data Inicio</param>
        /// <param name="dtFim">Data FIM</param>
        /// <param name="idEstado">ID Estado</param>
        /// <returns></returns>
        [HttpGet]
        public void Exportar(string dtInicio, string dtFim, string idEstado)
        {
            var lista = _repo.ListarAulaRealizada(dtInicio, dtFim, idEstado);
            if (lista.Count() > 0) ExportarResultado(Response, lista);
        }

        /// <summary>
        /// Exportar Aulas Por Simulador
        /// </summary>
        /// <param name="dtInicio">Data Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="idEstado">Id Estado</param>
        public void ExportarAulasPorSimulador(string dtInicio, string dtFim, string idEstado)
        {
            var lista = _repo.ListarAulaSimuladorAtivo(dtInicio, dtFim, idEstado);
            if (lista.Count() > 0) ExportarResultadoAulaPorSimulador(Response, lista);

        }

        //public void ExportarContratos(string dtInicio, string idEstado)
        //{
        //    var lista = _repo.ListarContratosFirmados(dtInicio, idEstado);
        //    if (lista.Count() > 0) ExportarResultadoContratos(Response, lista);
        //}

        /// <summary>
        /// Exportar Simuladores Ativos
        /// </summary>
        /// <param name="dtInicio">Data Inicio</param>
        /// <param name="dtFim">Data Fim</param>
        /// <param name="idEstado">Id Estado</param>
        /// <returns></returns>
        public void ExportarSimuladoresAtivos(string dtInicio, string dtFim, string idEstado)
        {
            var lista = _repo.ListarSimuladoresAtivos(dtInicio, dtFim, idEstado);
            if (lista.Count() > 0) ExportarResultadoSimuladoresAtivos(Response, lista);
        }


        private void ExportarResultadoSimuladoresAtivos(HttpResponseBase response, List<SimuladoresAtivos> lista)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=simuladoresativos.xls");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            byte[] preamble = System.Text.Encoding.UTF8.GetPreamble();
            Response.BinaryWrite(preamble);

            string strFontSize = "12";

            Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" \r\n xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \r\n xmlns=\"http://www.w3.org/TR/REC-html40\">");
            Response.Write("<head>");
            Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">");
            Response.Write("<meta name=ProgId content=Excel.Sheet>");
            Response.Write("<meta name=Generator content=\"Microsoft Excel 11\">");
            Response.Write("</head>");
            Response.Write("<body>");

            Response.Write("<table style='width: 100%; font-size:" + strFontSize + ";' border='1'>");
            Response.Write("<tr style='height:75px;'><td colspan='1'><img src='http://prointegracao.prosimulador.com.br/img/logo%20(1).png' style='position:absolute; top:0; left:0;z-index:-1;'/></td><td colspan='3'><h1>Simuladores Ativos</h1></td></tr>");

            DataTable dt = ConvertToDataTable<SimuladoresAtivos>(lista);

            Response.Write("<tr style='font-size:12px;'>");
            Response.Write("<td><b>ESTADO</b></td>");
            Response.Write("<td><b>RAZÃO SOCIAL</b></td>");
            Response.Write("<td><b>CNPJ</b></td>");
            Response.Write("<td><b>SMC</b></td>");
            Response.Write("</tr>");

            string strCell = "";
            var last = "";
            var subtotalsimulador = 0;
            var totalsimulador = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (last == row[1].ToString() || last == "")
                {
                    subtotalsimulador++;
                    totalsimulador++;


                    Response.Write("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName.ToUpper() != "DATA")
                        {
                            strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim();
                            Response.Write("<td>" + strCell + "</td>");
                        }

                    }

                    Response.Write("</tr>");


                }
                else
                {
                    Response.Write("<tr>");
                    Response.Write("<td colspan='3'style = 'font-weight:bold; background-color:#E5F7FD;'> SUBTOTAL DE AULAS</td>");
                    Response.Write("<td style = 'font-weight:bold; background-color:#E5F7FD;'class='text-center'>" + subtotalsimulador + "</td>");
                    Response.Write("</tr>");


                    Response.Write("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName.ToUpper() != "DATA")
                        {
                            strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim();
                            Response.Write("<td>" + strCell + "</td>");
                        }

                    }

                    Response.Write("</tr>");

                    subtotalsimulador = 1;
                    totalsimulador++;
                }
                last = row[1].ToString();
            }

            Response.Write("<tr>");
            Response.Write("<td colspan='3' style='font-weight:bold; background-color:#E5F7FD;'>SUBTOTAL DE SIMULADORES</td>");
            Response.Write("<td style = 'font-weight:bold; background-color:#E5F7FD;'class='text-center'>" + subtotalsimulador + "</td></tr>");
            Response.Write("</tr>");

            Response.Write("<tr>");
            Response.Write("<td colspan='3' style='font-weight:bold; background-color:#808080;color:#fff;'> TOTAL DE AULAS</td>");
            Response.Write("<td style='font-weight:bold; background-color:#808080;color:#fff;' class='text-center'> " + totalsimulador + "</td>");
            Response.Write("</tr>");

            Response.Write("</table>");
            Response.Write("</body>");
            Response.Write("</html>");

            Response.End();
        }

        private void ExportarResultadoContratos(HttpResponseBase response, List<ContratosFirmados> lista)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=contratos.xls");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            byte[] preamble = System.Text.Encoding.UTF8.GetPreamble();
            Response.BinaryWrite(preamble);

            string strFontSize = "12";

            Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" \r\n xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \r\n xmlns=\"http://www.w3.org/TR/REC-html40\">");
            Response.Write("<head>");
            Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">");
            Response.Write("<meta name=ProgId content=Excel.Sheet>");
            Response.Write("<meta name=Generator content=\"Microsoft Excel 11\">");
            Response.Write("</head>");
            Response.Write("<body>");

            Response.Write("<table style='width: 100%; font-size:" + strFontSize + ";' border='1'>");
            Response.Write("<tr style='height:75px;'><td colspan='1'><img src='http://prointegracao.prosimulador.com.br/img/logo%20(1).png' style='position:absolute; top:0; left:0;z-index:-1;'/></td><td colspan='3'><h1>Contratos Firmados</h1></td></tr>");

            DataTable dt = ConvertToDataTable<ContratosFirmados>(lista);

            Response.Write("<tr style='font-size:12px;'>");
            Response.Write("<td><b>DATA CONTRATO</b></td>");
            Response.Write("<td><b>ESTADO</b></td>");
            Response.Write("<td><b>NR. CONTRATOS</b></td>");
            Response.Write("<td><b>NR. SIMULADORES CONTRATADOS</b></td>");
            Response.Write("</tr>");

            string strCell = "";
            var totalcontratos = 0M;
            var totalsimuladores = 0M;

            foreach (DataRow row in dt.Rows)
            {
                totalcontratos += Convert.ToInt32(row[2].ToString());
                totalsimuladores += Convert.ToInt32(row[3].ToString());

                Response.Write("<tr>");

                foreach (DataColumn column in dt.Columns)
                {

                    strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim();
                    Response.Write("<td>" + strCell + "</td>");

                }

                Response.Write("</tr>");
            }

            Response.Write("<tr>");
            Response.Write("<td colspan='2' style='font-weight:bold; background-color:#808080;color:#fff;'> TOTAL DE AULAS</td>");
            Response.Write("<td style='font-weight:bold; background-color:#808080;color:#fff;' class='text-center'> " + totalcontratos + "</td>");
            Response.Write("<td style='font-weight:bold; background-color:#808080;color:#fff;' class='text-center'> " + totalsimuladores + "</td>");
            Response.Write("</tr>");

            Response.Write("</table>");
            Response.Write("</body>");
            Response.Write("</html>");

            Response.End();

        }

        /// <summary>
        /// Exportar Resultado
        /// </summary>
        /// <param name="Response"></param>
        /// <param name="clientsList"></param>
        public void ExportarResultado(HttpResponseBase Response, List<AulaRealizada> clientsList)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=aularealizada.xls");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            byte[] preamble = System.Text.Encoding.UTF8.GetPreamble();
            Response.BinaryWrite(preamble);

            string strFontSize = "12";

            Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" \r\n xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \r\n xmlns=\"http://www.w3.org/TR/REC-html40\">");
            Response.Write("<head>");
            Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">");
            Response.Write("<meta name=ProgId content=Excel.Sheet>");
            Response.Write("<meta name=Generator content=\"Microsoft Excel 11\">");
            Response.Write("</head>");
            Response.Write("<body>");

            Response.Write("<table style='width: 100%; font-size:" + strFontSize + ";' border='1'>");
            Response.Write("<tr style='height:75px;'><td colspan='1'><img src='http://prointegracao.prosimulador.com.br/img/logo%20(1).png' style='position:absolute; top:0; left:0;z-index:-1;'/></td><td colspan='5'><h1>Aulas Realizadas</h1></td></tr>");

            DataTable dt = ConvertToDataTable<AulaRealizada>(clientsList);

            Response.Write("<tr style='font-size:12px;'>");
            Response.Write("<td><b>DATA</b></td>");
            Response.Write("<td><b>SMC</b></td>");
            Response.Write("<td><b>CNPJ</b></td>");
            Response.Write("<td><b>RAZÃO SOCIAL</b></td>");
            Response.Write("<td><b>UF</b></td>");
            Response.Write("<td><b>AULAS REALIZADAS</b></td>");
            Response.Write("</tr>");

            string strCell = "";
            var subtotalaulas = 0;
            var totalaulas = 0;

            var last = "";
            var date = DateTime.Now;

            foreach (DataRow row in dt.Rows)
            {
                if ((last == row[4].ToString() && date == Convert.ToDateTime(row[0].ToString())) || last == "")
                {
                    subtotalaulas += Convert.ToInt32(row[5]);
                    totalaulas += Convert.ToInt32(row[5]);

                    Response.Write("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName.ToUpper() != "DATA")
                            strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim();
                        else
                            strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim().Substring(0, 10);



                        Response.Write("<td>" + strCell + "</td>");
                    }

                    Response.Write("</tr>");
                }
                else
                {

                    Response.Write("<tr>");
                    Response.Write("<td colspan='5'style = 'font-weight:bold; background-color:#E5F7FD;'> SUBTOTAL DE AULAS</td>");
                    Response.Write("<td style = 'font-weight:bold; background-color:#E5F7FD;'class='text-center'>" + subtotalaulas + "</td>");
                    Response.Write("</tr>");

                    Response.Write("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName.ToUpper() != "DATA")
                            strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim();
                        else
                            strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim().Substring(0, 10);

                        Response.Write("<td>" + strCell + "</td>");

                    }

                    Response.Write("</tr>");

                    subtotalaulas = Convert.ToInt32(row[5].ToString());
                    totalaulas += Convert.ToInt32(row[5]);
                }

                last = row[4].ToString();
                date = Convert.ToDateTime(row[0].ToString());

            }

            Response.Write("<tr>");
            Response.Write("<td colspan='5' style='font-weight:bold; background-color:#E5F7FD;'> SUBTOTAL DE AULAS</td>");
            Response.Write("<td style = 'font-weight:bold; background-color:#E5F7FD;'class='text-center'>" + subtotalaulas + "</td></tr>");
            Response.Write("</tr>");

            Response.Write("<tr>");
            Response.Write("<td colspan='5' style='font-weight:bold; background-color:#808080;color:#fff;'> TOTAL DE AULAS</td>");
            Response.Write("<td style='font-weight:bold; background-color:#808080;color:#fff;' class='text-center'> " + totalaulas + "</td>");
            Response.Write("</tr>");

            Response.Write("</table>");
            Response.Write("</body>");
            Response.Write("</html>");

            Response.End();
        }

        /// <summary>
        /// Exportar Resultado Aula PorSimulador
        /// </summary>
        /// <param name="response">Http Reponse Base</param>
        /// <param name="lista">Lista Aulas por Simulador</param>
        private void ExportarResultadoAulaPorSimulador(HttpResponseBase response, List<AulasPorSimulador> lista)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=aulasporsimulador.xls");
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.ContentType = "application/vnd.ms-excel";
            byte[] preamble = System.Text.Encoding.UTF8.GetPreamble();
            Response.BinaryWrite(preamble);

            string strFontSize = "12";

            Response.Write("<html xmlns:o=\"urn:schemas-microsoft-com:office:office\" \r\n xmlns:x=\"urn:schemas-microsoft-com:office:excel\" \r\n xmlns=\"http://www.w3.org/TR/REC-html40\">");
            Response.Write("<head>");
            Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=windows-1252\">");
            Response.Write("<meta name=ProgId content=Excel.Sheet>");
            Response.Write("<meta name=Generator content=\"Microsoft Excel 11\">");
            Response.Write("</head>");
            Response.Write("<body>");

            Response.Write("<table style='width: 100%; font-size:" + strFontSize + ";' border='1'>");
            Response.Write("<tr style='height:75px;'><td colspan='1'><img src='http://prointegracao.prosimulador.com.br/img/logo%20(1).png' style='position:absolute; top:0; left:0;z-index:-1;'/></td><td colspan='2'><h1>Média de Aulas Por Simulador</h1></td></tr>");

            DataTable dt = ConvertToDataTable<AulasPorSimulador>(lista);

            Response.Write("<tr style='font-size:12px;'>");
            Response.Write("<td><b>MES</b></td>");
            Response.Write("<td><b>ESTADO</b></td>");
            Response.Write("<td><b>MÉDIA</b></td>");
            Response.Write("</tr>");

            string strCell = "";
            var subtotalaulas = 0;
            var subtotalsimulador = 0;
            var totalaulas = 0M;
            var totalmes = 0M;
            var mes = "";
            var last = "";

            foreach (DataRow row in dt.Rows)
            {
                if (mes == "" || mes != row[0].ToString().Substring(0, 2))
                {
                    mes = row[0].ToString().Substring(0, 2);
                    totalmes++;
                }


                if (last == row[0].ToString() || last == "")
                {
                    subtotalaulas += Convert.ToInt32(row[2].ToString());
                    subtotalsimulador += Convert.ToInt32(row[3].ToString());

                    Response.Write("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName.ToUpper() != "NRMEDIOAULAS" && column.ColumnName.ToUpper() != "NRSIMULADORES")
                        {
                            strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim();
                            Response.Write("<td>" + strCell + "</td>");
                        }
                    }

                    Response.Write("</tr>");
                }
                else
                {

                    Response.Write("<tr>");
                    Response.Write("<td colspan='2'style = 'font-weight:bold; background-color:#E5F7FD;'> SUBTOTAL DE AULAS</td>");
                    Response.Write("<td style = 'font-weight:bold; background-color:#E5F7FD;'class='text-center'>" + string.Format("{0:N2}", (Convert.ToDecimal(subtotalaulas)) / (Convert.ToDecimal(subtotalsimulador))) + "</td>");
                    Response.Write("</tr>");

                    Response.Write("<tr>");

                    foreach (DataColumn column in dt.Columns)
                    {
                        if (column.ColumnName.ToUpper() != "NRMEDIOAULAS" && column.ColumnName.ToUpper() != "NRSIMULADORES")
                        {
                            strCell = row.ItemArray[dt.Columns[column.ColumnName].Ordinal].ToString().Trim();
                            Response.Write("<td>" + strCell + "</td>");
                        }
                    }

                    Response.Write("</tr>");

                    totalaulas += (Convert.ToDecimal(subtotalaulas)) / (Convert.ToDecimal(subtotalsimulador));

                    subtotalaulas = Convert.ToInt32(row[2].ToString());
                    subtotalsimulador = Convert.ToInt32(row[3].ToString());

                }

                last = row[0].ToString();
            }

            Response.Write("<tr>");
            Response.Write("<td colspan='2' style='font-weight:bold; background-color:#E5F7FD;'> SUBTOTAL DE AULAS</td>");
            Response.Write("<td style = 'font-weight:bold; background-color:#E5F7FD;'class='text-center'>" + string.Format("{0:N2}", (Convert.ToDecimal(subtotalaulas)) / (Convert.ToDecimal(subtotalsimulador))) + "</td></tr>");
            Response.Write("</tr>");

            totalaulas += (Convert.ToDecimal(subtotalaulas)) / (Convert.ToDecimal(subtotalsimulador));

            Response.Write("<tr>");
            Response.Write("<td colspan='2' style='font-weight:bold; background-color:#808080;color:#fff;'> TOTAL DE AULAS</td>");
            Response.Write("<td style='font-weight:bold; background-color:#808080;color:#fff;' class='text-center'> " + string.Format("{0:N2}", (Convert.ToDecimal(totalaulas)) / (Convert.ToDecimal(totalmes))) + "</td>");
            Response.Write("</tr>");

            Response.Write("</table>");
            Response.Write("</body>");
            Response.Write("</html>");

            Response.End();


        }


        #endregion

    }
}