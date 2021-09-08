using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult LabResultDate()
        {
            return View();
        }

        public ActionResult Result()
        {
            GetFileAndFolderFromSharepoint();

            return View();
        }

        public void GetFileAndFolderFromSharepoint()
        {
            string userName = "sittinon@zmedicgroup.com";
            string password = "1Q2w3e4r";
            var securePassword = new SecureString();
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
                List<string> lstFile = new List<string>();
                List<string> lstFileName = new List<string>();
                List<string> guestFileUrl = new List<string>();
                foreach (Folder f in fcol)
                {
                    if (f.Name == "COVID")
                    {
                        cxt.Load(f.Files);
                        cxt.ExecuteQuery();
                        FileCollection fileCol = f.Files;
                        foreach (File file in fileCol)
                        {
                            lstFileName.Add(file.Name);
                            lstFile.Add("https://zmedicgroup.sharepoint.com" + file.ServerRelativeUrl);
                        }
                    }

                    ViewBag.lstFile = lstFile;
                    ViewBag.lstFileName = lstFileName;
                }
            }
        }
    }
}