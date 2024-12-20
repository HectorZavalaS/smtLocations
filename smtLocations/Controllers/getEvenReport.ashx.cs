﻿using smtLocations.Class;
using smtLocations.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace smtLocations.Controllers
{
    /// <summary>
    /// Summary description for getEvenReport
    /// </summary>
    public class getEvenReport : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            siixsem_smtLocations_dbEntities m_db = new siixsem_smtLocations_dbEntities();
            String pathReport = "";
            excel m_excel = new excel();
            String json = "{";
            CUtils utils = new CUtils();

            DataTable evens = utils.ToDataTable(m_db.getEvenReels().ToList());

            String fileName = "Even_Reels_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".xlsx";
            
            m_excel.write_fileOLE(evens, fileName, context.Server.MapPath("~/Reports/Even"), ref pathReport);
            json += "\"result\":\"true\",";
            json += "\"html\":\"" + fileName + "\"";
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