using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using Zmedic.Models;


namespace Zmedic.Controllers
{
    public class PatientController : Controller
    {
        // GET: Patient
        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult PatientLabSearch()
        {
            return View();
        }

        public ActionResult LabResultDate(string dateInput, string sixIdInput)
        {
            DateTime dateTime = DateTime.Parse(dateInput);

            DateTime dateTimeAddYears = dateTime.AddYears(543);

            string dateOfBirthFileName = dateTimeAddYears.ToString("ddMMyyyy");

            GetFileAndFolderFromSharepoint(dateOfBirthFileName + sixIdInput);

            return View();
        }

        public ActionResult Result(string urlPdf)
        {
            ViewBag.urlPdf = urlPdf;

            return View();
        }

        public void GetFileAndFolderFromSharepoint(string pdfFileName)
        {
            string userName = "sittinon@zmedicgroup.com";
            string password = "1Q2w3e4r";
            var securePassword = new SecureString();

            try
            {
                foreach (char c in password)
                {
                    securePassword.AppendChar(c);
                }
                using (ClientContext cxt = new ClientContext("https://zmedicgroup.sharepoint.com/sites/ACCRESULT"))
                {
                    cxt.Credentials = new SharePointOnlineCredentials(userName, securePassword);
                    Web web = cxt.Web;
                    cxt.Load(web, a => a.ServerRelativeUrl);
                    cxt.ExecuteQuery();
                    List list = cxt.Web.Lists.GetByTitle("เอกสาร");
                    cxt.Load(list);
                    cxt.Load(list.RootFolder);
                    cxt.Load(list.RootFolder.Folders);
                    cxt.Load(list.RootFolder.Files);
                    cxt.ExecuteQuery();
                    FolderCollection fcol = list.RootFolder.Folders;
                    List<FilePDF> filePDFs = new List<FilePDF>();

                    foreach (Folder f in fcol)
                    {
                        if (f.Name == "COVID")
                        {
                            cxt.Load(f.Files);
                            cxt.ExecuteQuery();
                            FileCollection fileCol = f.Files;
                            foreach (File file in fileCol)
                            {
                                if (file.Name.StartsWith(pdfFileName))
                                {
                                    filePDFs.Add(new FilePDF { FilePdfName = file.Name, FilePdfUrl = "https://zmedicgroup.sharepoint.com" + file.ServerRelativeUrl });
                                }
                            }
                        }

                        if (filePDFs.Count == 0)
                        {
                            ViewBag.Nodata = "ไม่พบผลการตรวจ";
                        }

                        ViewBag.lstFile = filePDFs;
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ex = ex;
            }
        }
    }
}


