using smtLocations.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace smtLocations.Controllers
{
    /// <summary>
    /// Summary description for getLocations
    /// </summary>
    public class getLocations : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            String json = "{";
            try
            {
                String htmlHead = "";
                String htmlBody = "";
                String html = "";
                String strReels = "";
                String rowExpand = "";
                int count = 0;
                
                siixsem_smtLocations_dbEntities m_db = new siixsem_smtLocations_dbEntities();
                var locations = m_db.getSMKTLocations();

                String model = "";
                int reqQty = 0;
                int curQty = 0;
                

                html += "<table id='tableLoc' class='table stripe display' style='width:100%;'>";

                htmlHead = "<thead>";
                htmlHead += "<tr>";

                htmlHead += "<th>TABLE NAME</th>";
                htmlHead += "<th>REEL SIZE</th>";
                htmlHead += "<th>PART NUMBER</th>";
                htmlHead += "<th>MAX QTY</th>";
                htmlHead += "<th>CURRENT QTY</th>";
                htmlHead += "<th>STATUS</th>";
                htmlHead += "<th>ELAPSED TIME IN 0</th>";
                htmlHead += "<th></th>";

                htmlHead += "</tr>";
                htmlHead += "</thead>";

                htmlBody += "<tbody>";
                int i = 0;
                foreach (getSMKTLocations_Result row in locations)
                {
                    if(row.EXPIRED == 1)
                        htmlBody += "<tr style='background-color:black; color: white;' ";
                    else
                        if (row.CURRENT_QTY == 0)
                            htmlBody += "<tr class='parpadea' style='background-color:yellow; color: black;' ";
                        else
                            if (row.CURRENT_QTY == row.MAX_QTY)
                                htmlBody += "<tr style='background-color:green; color: white;' ";
                            else
                                if (row.CURRENT_QTY < row.MAX_QTY)
                                    htmlBody += "<tr class='parpadea' style='background-color:yellow; color: black;' ";
                                else
                                    if(row.CURRENT_QTY > row.MAX_QTY)
                                        htmlBody += "<tr style='background-color:orange; color: white;' ";

                    var reels = m_db.getReelsByLoc(row.PART_NUMBER.Replace("\r\n", ""));


                    if (reels!=null)
                    {
                        foreach(getReelsByLoc_Result reel in reels)
                        {
                            strReels += "<label>* ";
                            strReels += reel.ITEM_ID + ": " + (reel.EXPIRED == 0 ? "VIGENTE" : "EXPIRADO");
                            strReels += "   " + " - INIT QTY: " + reel.INIT_QTY.ToString();
                            strReels += "   " + " - CURR QTY: " + reel.CURR_QTY.ToString();
                            strReels += "</label><br>";
                        }
                        if (!String.IsNullOrEmpty(strReels))
                        {
                            htmlBody += "data-child-value='" + strReels + "'>";
                            rowExpand = "<td class='details-control'></td>";
                        }
                        else
                        {
                            htmlBody += ">";
                            rowExpand = "<td></td>";
                        }
                        strReels = "";
                    }
                    else
                    {
                        htmlBody += ">";
                    }

                    
                    htmlBody += "<td style='font-weight:bold;'>" + row.TABLE_NAME.Replace("\r\n", "") + "</td>";
                    htmlBody += "<td style='font-weight:bold;'><label>" + row.REEL_SIZE.Replace("\"","in").Replace("\r\n","") + "</label></td>";
                    htmlBody += "<td style='font-weight:bold;'><label>" + row.PART_NUMBER.Replace("\r\n", "") + "</label></td>";
                    htmlBody += "<td style='font-weight:bold;'><label>" + row.MAX_QTY.ToString() + "</label></td>";
                    htmlBody += "<td style='font-weight:bold;'><label>" + row.CURRENT_QTY.ToString() + "</label></td>";
                    DateTime tmp = Convert.ToDateTime(row.LAST_UPDATE);
                        
                    reqQty+= row.MAX_QTY;
                    curQty+= row.CURRENT_QTY;
                    if (row.EXPIRED == 1)
                        htmlBody += "<td style='font-weight:bold;'><label>EXPIRED</label></td>";
                    else
                        htmlBody += "<td style='font-weight:bold;'><label>VIGENT</label></td>";
                    htmlBody += "<td style='font-weight:bold;'><label>" + (DateTime.Now - tmp).ToString(@"dd\d\ hh\h\ mm\m\ ") + "</label></td>";
                    htmlBody += rowExpand;
                    htmlBody += "</tr>";
                    i++;
                }
                htmlBody += "</tbody>";


                html += htmlHead + htmlBody + "</table>";
                json += "\"reqQty\":\"" + reqQty.ToString() + "\",";
                json += "\"curQty\":\"" + curQty.ToString() + "\",";
                json += "\"result\":\"true\",";
                json += "\"html\":\"" + html + "\"";
            }
            catch (Exception ex) { }
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