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
    public partial class genereicReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            ReportViewer2.Reset();

            ReportViewer2.ProcessingMode = ProcessingMode.Local;
            ReportViewer2.LocalReport.ReportPath = Server.MapPath("genericReport.rdlc");
            string schoolID = Session["currentSchool"].ToString();
            DataTable dt = GetData(schoolID);
            ReportDataSource rds = new ReportDataSource("InvoiceSchool", dt);

            ReportViewer2.LocalReport.DataSources.Add(rds);

            ReportViewer2.SizeToReportContent = true;
            //ReportViewer1.ZoomMode = ZoomMode.FullPage;

            ReportViewer2.LocalReport.EnableExternalImages = true;
            
            string imagePath = new Uri(Server.MapPath($"~/Logos/{schoolID}.png")).AbsoluteUri;

            ReportParameter parameter = new ReportParameter("School", imagePath);
            ReportViewer2.LocalReport.SetParameters(parameter);
            ReportViewer2.LocalReport.Refresh();


        }

        private DataTable GetData(string school)
        {
            DataTable dt = new DataTable();

            string connStr = ConfigurationManager.ConnectionStrings["Student_Affairs_DatabaseConnectionString1"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("dbo.getSchoolInvoiceReport", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@School", SqlDbType.Int).Value = Int32.Parse(school);
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

       
    }
}