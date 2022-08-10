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
    public partial class Payment : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            ReportViewer4.Reset();

            ReportViewer4.ProcessingMode = ProcessingMode.Local;
            ReportViewer4.LocalReport.ReportPath = Server.MapPath("Payment_Invoice.rdlc");

            DataTable dt = GetData(Request.QueryString["invoice"]);
            ReportDataSource rds = new ReportDataSource("Payment", dt);
            
            ReportViewer4.LocalReport.DataSources.Add(rds);

            ReportViewer4.SizeToReportContent = true;
            //ReportViewer4.ZoomMode = ZoomMode.FullPage;

            ReportViewer4.LocalReport.EnableExternalImages = true;
            string schoolID = Session["currentSchool"].ToString();
            string imagePath = new Uri(Server.MapPath($"~/Logos/{schoolID}.png")).AbsoluteUri;
            StudentAffairsDatabaseEntities db = new StudentAffairsDatabaseEntities();
        invoice_payment inv = db.invoice_payment.Find(Int32.Parse(Request.QueryString["invoice"]));
        ReportParameter[] Rptparams = new ReportParameter[] { new ReportParameter("School", schoolID),
                                                                new ReportParameter("Paid", inv.paid.ToString()),
                                                                new ReportParameter("Remaining", inv.remaining.ToString()),
                                                              new ReportParameter("TotalCost", inv.total_cost.ToString())};
            ReportViewer4.LocalReport.SetParameters(Rptparams);
            ReportViewer4.LocalReport.Refresh();
        }

        private DataTable GetData(string invoice)
        {
            DataTable dt = new DataTable();

            string connStr = ConfigurationManager.ConnectionStrings["Student_Affairs_DatabaseConnectionString1"].ConnectionString;
            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    con.Open();

                    using (SqlCommand cmd = new SqlCommand("dbo.getFeeInvoice", con))
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