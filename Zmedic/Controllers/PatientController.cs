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
    [HandleError]
    public class PatientController : Controller
    {
        AccZmedicEntities _context = new AccZmedicEntities();

        // GET: Patient
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //------------------------------------- ค้นหาผลแลป -------------------------------------------------------
        public ActionResult PatientLabSearch()
        {
            return View();
        }

        public ActionResult LabResultDate(string dateInput, string sixIdInput)
        {

            try
            {
                DateTime date = DateTime.Parse(dateInput).Date;

                //DateTime dateTimeAddYears = dateTime.AddYears(543);

                string dateOfBirth = date.ToString("d/M/yyyy");

                var patientReport = _context.Patient;

                var patientReportResult = patientReport.Where(p => p.Data_Status == true
                && p.DOB == dateOfBirth
                && p.ID_Passport.Contains(sixIdInput)).ToList();

                if (patientReportResult.Count == 0)
                {
                    ViewBag.Nodata = "ไม่พบผลการตรวจ";
                }


                return View(patientReportResult);
            }

            catch (Exception ex)
            {
                ViewBag.ex = ex;

                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Result(string fileNamePdf)
        {
            string getFileServerRelativeUrl = GetFileAndFolderFromSharepoint(fileNamePdf);

            string filePdf = DownloadFileFromSharepointToLocalServer(getFileServerRelativeUrl);

            ViewBag.filePdf = filePdf;

            return View();
        }

        //------------------------------------- เคลียร์ไฟล์PDF -------------------------------------------------------

        public ActionResult ClearPdfFileTemp()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                var path = Server.MapPath("~/pdfTempfile");
                try
                {
                    System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(path);

                    foreach (System.IO.FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (System.IO.DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ex = ex;
                }
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //------------------------------------- SharePoint -------------------------------------------------------

        public string GetFileAndFolderFromSharepoint(string pdfFileName)
        {
            string userName = "sittinon@zmedicgroup.com";
            string password = "1Q2w3e4r";
            var securePassword = new SecureString();
            string fileServerRelativeUrl = null;

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
                                    fileServerRelativeUrl = file.ServerRelativeUrl;
                                }
                            }
                        }
                    }

                }
                return fileServerRelativeUrl;
            }
            catch (Exception ex)
            {
                ViewBag.ex = ex;
                return fileServerRelativeUrl;
            }
        }

        public string DownloadFileFromSharepointToLocalServer(string serverRelativeUrl)
        {
            string userName = "sittinon@zmedicgroup.com";
            string password = "1Q2w3e4r";
            var securePassword = new SecureString();

            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }

            ClientContext clientContext = new ClientContext("https://zmedicgroup.sharepoint.com/sites/ACCRESULT");
            clientContext.Credentials = new SharePointOnlineCredentials(userName, securePassword);
            Web web = clientContext.Web;
            Microsoft.SharePoint.Client.File filetoDownload = web.GetFileByServerRelativeUrl(serverRelativeUrl);
            clientContext.Load(filetoDownload);
            clientContext.ExecuteQuery();
            var fileRef = filetoDownload.ServerRelativeUrl;
            var fileInfo = Microsoft.SharePoint.Client.File.OpenBinaryDirect(clientContext, fileRef);
            var path = Server.MapPath("~/pdfTempfile");
            var fileName = System.IO.Path.Combine(path, (string)filetoDownload.Name);
            var fileNameURL = filetoDownload.Name;

            using (var fileStream = System.IO.File.Create(fileName))
            {
                fileInfo.Stream.CopyTo(fileStream);
            }

            return fileNameURL;
        }
    }
}


