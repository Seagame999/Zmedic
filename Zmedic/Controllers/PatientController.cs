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
        AccZmedicEntities _context = new AccZmedicEntities();

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
            try
            {
                DateTime dateTime = DateTime.Parse(dateInput);

                DateTime dateTimeAddYears = dateTime.AddYears(543);

                string dateOfBirthFileName = dateTimeAddYears.ToString("ddMMyyyy");

                GetFileAndFolderFromSharepoint(dateOfBirthFileName + sixIdInput);

            }
            catch (Exception ex)
            {
                ViewBag.ex = ex;
            }
            return View();
        }

        public ActionResult Result(string serverRelativeUrl)
        {
            string filePdf = DownloadFileFromSharepointToLocalServer(serverRelativeUrl);

            ViewBag.filePdf = filePdf;

            return View();
        }

        public ActionResult ClearPdfFileTemp()
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
            catch(Exception ex)
            {
                ViewBag.ex = ex;
            }
            
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
                                    filePDFs.Add(new FilePDF { FilePdfName = file.Name, FilePdfUrl = file.ServerRelativeUrl });
                                }
                            }
                        }

                        ViewBag.lstFile = filePDFs;
                    }

                    if (filePDFs.Count == 0)
                    {
                        ViewBag.Nodata = "ไม่พบผลการตรวจ";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ex = ex;
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


