using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using StudentsAffairsDashboard.Models;

namespace StudentsAffairsDashboard.Reports
{
    public partial class soldReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            


        }

        private DataTable GetData(string school, DateTime fromD, DateTime toD)
        {
            DataTable dt = new DataTable();

            string connStr = ConfigurationManager.ConnectionStrings["Student_Affairs_DatabaseConnectionString1"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("dbo.getSoldBetween", con))
                    {
                        
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@school", SqlDbType.Int).Value = Int32.Parse(school);
                        cmd.Parameters.Add("@from", SqlDbType.Date).Value = fromD.Date;
                        cmd.Parameters.Add("@to", SqlDbType.Date).Value = toD.Date;
                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        adp.Fill(dt);
                    }
                    con.Close();
                    return dt;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        protected void btnGet_Click(object sender, EventArgs e)
        {
            ReportViewer3.Reset();

            ReportViewer3.ProcessingMode = ProcessingMode.Local;
            ReportViewer3.LocalReport.ReportPath = Server.MapPath("soldReport.rdlc");
            //string schoolID = Session["currentSchool"].ToString();
            string schoolID = "1001";
            string type = Request.QueryString["Type"];
            var fromD = fromDate.SelectedDate;
            var toD = toDate.SelectedDate;
            DataTable dt = GetData(schoolID, fromD, toD);
            ReportDataSource rds = new ReportDataSource("getSales", dt);

            ReportViewer3.LocalReport.DataSources.Add(rds);

            ReportViewer3.SizeToReportContent = true;
            //ReportViewer1.ZoomMode = ZoomMode.FullPage;

            ReportViewer3.LocalReport.EnableExternalImages = true;

            string imagePath = new Uri(Server.MapPath($"~/Logos/{schoolID}.png")).AbsoluteUri;
            ReportParameter[] Rptparams = new ReportParameter[] { new ReportParameter("School", imagePath),
                                                              new ReportParameter("Type", type)};
            ReportViewer3.LocalReport.SetParameters(Rptparams);
            ReportViewer3.LocalReport.Refresh();
        }

    }
    
}