using LMS.Models;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class PdfController : Controller
    {
        public DbModels db = new DbModels();
        // GET: Pdf
        public ActionResult Index()
        {
            var id = Convert.ToInt32(Session["Id"].ToString());
            var list = db.Transactions.Where(a => a.RequestId == id).ToList();
            return View(list);
        }
        public ActionResult Reports(string ReportType,int RequestId)
        {
            var id = Convert.ToInt32(Session["Id"].ToString());
            LocalReport localreport = new LocalReport();
            localreport.ReportPath = Server.MapPath("~/Reports/LaundryReport.rdlc");
            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "DataSet1";
            reportDataSource.Value = db.Transactions.Where(a => a.RequestId == id).ToList();
            localreport.DataSources.Add(reportDataSource);
            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;
            if(reportType == "Excel")
            {
                fileNameExtension = ".xlsx";
            }
            else if (reportType == "word")
            {
                fileNameExtension = ".docx";
            }
            else if (reportType == "PDF")
            {
                fileNameExtension = ".pdf";
            }
            else 
            {
                fileNameExtension = ".jpg";
            }
            string[] streams;
            Warning[] warnings;
            byte[] renderedByte;
            renderedByte = localreport.Render(reportType, "", out mimeType, out encoding, out fileNameExtension, out streams, out warnings);
            Response.AppendHeader("content-Disposition", "attachment; filename=RDLC." + fileNameExtension);
            return File(renderedByte,fileNameExtension);
            
        }
    }
}