using smtLocations.Class;
using smtLocations.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace smtLocations.Controllers
{
    /// <summary>
    /// Summary description for sendMail
    /// </summary>
    public class sendMail : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {

            siixsem_smtLocations_dbEntities m_db = new siixsem_smtLocations_dbEntities();
            String pathReportEven = "";
            String pathReportOdd = "";
            excel m_excel = new excel();
            String json = "{";
            CUtils utils = new CUtils();


            ///////// obtiene el reporte de pares actualizado
            DataTable evens = utils.ToDataTable(m_db.getEvenReels().ToList());

            String fileNameEven = "Even_Reels_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";

            m_excel.write_fileOLE(evens, fileNameEven, context.Server.MapPath("~/Reports/Even"), ref pathReportEven);

            ///////// obtiene el reporte de impares actualizado
            evens = utils.ToDataTable(m_db.getOddReels().ToList());

            String fileNameOdd = "Odd_Reels_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";

            m_excel.write_fileOLE(evens, fileNameOdd, context.Server.MapPath("~/Reports/Odd"), ref pathReportOdd);

            List<string> lstArchivos = new List<string>();
            lstArchivos.Add(pathReportEven);
            lstArchivos.Add(pathReportOdd);

            //creamos nuestro objeto de la clase que hicimos
            CMail oMail = new CMail("smtlocations@siix.mx", "cristobal.munoz@siix-sem.com.mx",
                                "Even & Odd Reports", "Even & Odd Reports", lstArchivos);

            oMail.Message = "Se adjuntan Reportes de Locaciones pares e impares.";

            //y enviamos
            if (oMail.enviaMail())
            {
                json += "\"result\":\"true\",";
                json += "\"html\":\"" + "Se envio el correo." + "\"";

            }
            else
            {
                //Console.Write("no se envio el mail: " + oMail.error);
                json += "\"result\":\"false\",";
                json += "\"html\":\"" + "No se envio el correo. " + oMail.error + "\"";

            }
            json += "}";
            context.Response.ContentType = "text/plain";
            context.Response.Write(json);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}