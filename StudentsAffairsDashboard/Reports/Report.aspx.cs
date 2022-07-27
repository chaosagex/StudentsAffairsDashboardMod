using System;
using System.Collections.Generic;
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
            ReportViewer1.ProcessingMode = ProcessingMode.Local;
            ReportViewer1.LocalReport.ReportPath = Server.MapPath("Report1.rdlc");
            StudentAffairsDatabaseEntities entities = new StudentAffairsDatabaseEntities();
            ReportDataSource datasource = new ReportDataSource("Invoices", entities.invoice_payment.ToList());
            ReportViewer1.LocalReport.DataSources.Clear();
            ReportViewer1.LocalReport.DataSources.Add(datasource);
            ReportViewer1.SizeToReportContent = true;
            ReportViewer1.ZoomMode = ZoomMode.FullPage;
            ReportViewer1.LocalReport.EnableExternalImages = true;
            string imagePath = new Uri(Server.MapPath("~/Logos/1007.png")).AbsoluteUri;
            ReportParameter parameter = new ReportParameter("School", imagePath);
            ReportViewer1.LocalReport.SetParameters(parameter);
            ReportViewer1.LocalReport.Refresh();
        }
    }
}