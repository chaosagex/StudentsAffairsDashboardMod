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
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ReportViewer1.Reset();

            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Uniform_Invoice.rdlc");

            DataTable dt = GetData(Request.QueryString["invoice"]);
            ReportDataSource rds = new ReportDataSource("Uniforms", dt);

            ReportViewer1.LocalReport.DataSources.Add(rds);

            ReportViewer1.SizeToReportContent = true;
            //ReportViewer1.ZoomMode = ZoomMode.FullPage;

            ReportViewer1.LocalReport.EnableExternalImages = true;
            string schoolID = Session["currentSchool"].ToString();
            string imagePath = new Uri(Server.MapPath($"~/Logos/{schoolID}.png")).AbsoluteUri;

            ReportParameter parameter = new ReportParameter("School", imagePath);
            ReportViewer1.LocalReport.SetParameters(parameter);
            ReportViewer1.LocalReport.Refresh();


        }

        private DataTable GetData(string invoice)
        {
            DataTable dt = new DataTable();

            string connStr = ConfigurationManager.ConnectionStrings["Student_Affairs_DatabaseConnectionString"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("dbo.getUniformByInvoice", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@invoice", SqlDbType.Int).Value = Int32.Parse(invoice);
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