using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using OfficeOpenXml.Table;
using System.IO;
using Zmedic.Models;

namespace Zmedic.Controllers
{
    [HandleError]
    public class AdminPanelController : Controller
    {
        AccZmedicEntities _context = new AccZmedicEntities();

        //------------------------------------- หน้าหลัก -------------------------------------------------------

        public ActionResult Index()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //------------------------------------- นำเข้า Excel -------------------------------------------------------

        public ActionResult ImportExcelFile()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult ImportExcelFile(FormCollection formCollection)
        {
            var masterList = new List<Master_template>();
            var patientList = new List<Patient>();
            var logUpload = new Upload();

            if (Request != null)
            {
                HttpPostedFileBase file = Request.Files["ExcelFile"];

                if ((file != null) &&
                    (file.FileName.EndsWith(".xls")) || (file.FileName.EndsWith(".XLS")) || (file.FileName.EndsWith(".xlsx")) || (file.FileName.EndsWith(".XLSX"))
                    && (file.ContentLength > 0) && !string.IsNullOrEmpty(file.FileName))
                {
                    string fileName = file.FileName;
                    string fileContentType = file.ContentType;
                    byte[] fileBytes = new byte[file.ContentLength];

                    var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));

                    using (var package = new ExcelPackage(file.InputStream))
                    {
                        var currentSheet = package.Workbook.Worksheets;
                        var workSheet = currentSheet.First();
                        var numberOfCol = workSheet.Dimension.End.Column;
                        var numberOfRow = workSheet.Dimension.End.Row;

                        for (int rowIterator = 2; rowIterator <= numberOfRow; rowIterator++)
                        {
                            var masterTemplate = new Master_template();
                            var patient = new Patient();

                            //Data_Status_in_DB
                            masterTemplate.Data_Status = true;
                            patient.Data_Status = true;

                            //Col_Number
                            if (workSheet.Cells[rowIterator, 1].Value == null)
                            {
                                masterTemplate.Number = null;
                            }
                            else
                            {
                                masterTemplate.Number = Convert.ToInt32(workSheet.Cells[rowIterator, 1].Value);
                            }

                            //Col_Prefix
                            if (workSheet.Cells[rowIterator, 2].Value == null)
                            {
                                masterTemplate.Prefix = null;
                            }
                            else
                            {
                                masterTemplate.Prefix = workSheet.Cells[rowIterator, 2].Value.ToString();
                            }

                            //Col_FirstName
                            if (workSheet.Cells[rowIterator, 3].Value == null)
                            {
                                masterTemplate.First_Name = null;
                            }
                            else
                            {
                                masterTemplate.First_Name = workSheet.Cells[rowIterator, 3].Value.ToString();
                            }

                            //Col_LastName
                            if (workSheet.Cells[rowIterator, 4].Value == null)
                            {
                                masterTemplate.Last_Name = null;
                            }
                            else
                            {
                                masterTemplate.Last_Name = workSheet.Cells[rowIterator, 4].Value.ToString();
                            }

                            //Col_IdCard
                            if (workSheet.Cells[rowIterator, 5].Value == null)
                            {
                                return RedirectToAction("NullAlert", "AdminPanel");
                                //masterTemplate.ID_Passport = null;
                                //patient.ID_Passport = null;
                            }
                            else
                            {
                                masterTemplate.ID_Passport = workSheet.Cells[rowIterator, 5].Value.ToString();
                                patient.ID_Passport = workSheet.Cells[rowIterator, 5].Value.ToString();
                            }

                            //Col_DOB
                            if (workSheet.Cells[rowIterator, 6].Value == null)
                            {
                                return RedirectToAction("NullAlert", "AdminPanel");
                                //masterTemplate.DOB = null;
                                //patient.DOB = null;
                            }
                            else
                            {
                                masterTemplate.DOB = Convert.ToDateTime(workSheet.Cells[rowIterator, 6].Value).Date.AddYears(543);
                                patient.DOB = workSheet.Cells[rowIterator, 6].Value.ToString();
                            }

                            //Col_Age
                            if (workSheet.Cells[rowIterator, 7].Value == null)
                            {
                                masterTemplate.AGE = null;
                            }
                            else
                            {
                                masterTemplate.AGE = workSheet.Cells[rowIterator, 7].Value.ToString();
                            }

                            //Col_Sex
                            if (workSheet.Cells[rowIterator, 8].Value == null)
                            {
                                masterTemplate.Sex = null;
                            }
                            else
                            {
                                masterTemplate.Sex = workSheet.Cells[rowIterator, 8].Value.ToString();
                            }

                            //Col_Collected_date
                            if (workSheet.Cells[rowIterator, 9].Value == null)
                            {
                                masterTemplate.Collected_Date = null;
                                patient.Collected_Date = null;
                            }
                            else
                            {
                                masterTemplate.Collected_Date = Convert.ToDateTime(workSheet.Cells[rowIterator, 9].Value);
                                patient.Collected_Date = Convert.ToDateTime(workSheet.Cells[rowIterator, 9].Value);
                            }

                            //Col_Specimen
                            if (workSheet.Cells[rowIterator, 10].Value == null)
                            {
                                masterTemplate.Specimen = null;
                            }
                            else
                            {
                                masterTemplate.Specimen = workSheet.Cells[rowIterator, 10].Value.ToString();
                            }

                            //Col_HospitalClinic
                            if (workSheet.Cells[rowIterator, 11].Value == null)
                            {
                                masterTemplate.Hospital_Clinic = null;
                            }
                            else
                            {
                                masterTemplate.Hospital_Clinic = workSheet.Cells[rowIterator, 11].Value.ToString();
                            }

                            //Col_ReceivedDate
                            if (workSheet.Cells[rowIterator, 12].Value == null)
                            {
                                masterTemplate.Received_Date = null;
                            }
                            else
                            {
                                masterTemplate.Received_Date = Convert.ToDateTime(workSheet.Cells[rowIterator, 12].Value);
                            }

                            //Col_Doctor
                            if (workSheet.Cells[rowIterator, 13].Value == null)
                            {
                                masterTemplate.Doctor = null;
                            }
                            else
                            {
                                masterTemplate.Doctor = workSheet.Cells[rowIterator, 13].Value.ToString();
                            }

                            //Col_VN
                            if (workSheet.Cells[rowIterator, 14].Value == null)
                            {
                                masterTemplate.VN = null;
                            }
                            else
                            {
                                masterTemplate.VN = workSheet.Cells[rowIterator, 14].Value.ToString();
                            }

                            //Col_LN
                            if (workSheet.Cells[rowIterator, 15].Value == null)
                            {
                                return RedirectToAction("NullAlert", "AdminPanel");
                                //masterTemplate.LN = null;
                                //patient.LN = null;
                            }
                            else
                            {
                                masterTemplate.LN = workSheet.Cells[rowIterator, 15].Value.ToString();
                                patient.LN = workSheet.Cells[rowIterator, 15].Value.ToString();
                            }

                            //Col_HN
                            if (workSheet.Cells[rowIterator, 16].Value == null)
                            {
                                masterTemplate.HN = null;
                            }
                            else
                            {
                                masterTemplate.HN = workSheet.Cells[rowIterator, 16].Value.ToString();
                            }

                            //Col_N_gene
                            if (workSheet.Cells[rowIterator, 17].Value == null)
                            {
                                masterTemplate.N_gene_Ct = null;
                            }
                            else
                            {
                                masterTemplate.N_gene_Ct = workSheet.Cells[rowIterator, 17].Value.ToString();
                            }

                            //Col_S_gene
                            if (workSheet.Cells[rowIterator, 18].Value == null)
                            {
                                masterTemplate.S_gene_Ct = null;
                            }
                            else
                            {
                                masterTemplate.S_gene_Ct = workSheet.Cells[rowIterator, 18].Value.ToString();
                            }

                            //Col_Date(Start)
                            if (workSheet.Cells[rowIterator, 19].Value == null)
                            {
                                masterTemplate.Date_start = null;
                            }
                            else
                            {
                                masterTemplate.Date_start = workSheet.Cells[rowIterator, 19].Value.ToString();
                            }

                            //Col_Time(Start)
                            if (workSheet.Cells[rowIterator, 20].Value == null)
                            {
                                masterTemplate.Time_Start = null;
                            }
                            else
                            {
                                masterTemplate.Time_Start = workSheet.Cells[rowIterator, 20].Value.ToString();
                            }

                            //Col_Date(finish)
                            if (workSheet.Cells[rowIterator, 21].Value == null)
                            {
                                masterTemplate.Date_Finish = null;
                            }
                            else
                            {
                                masterTemplate.Date_Finish = workSheet.Cells[rowIterator, 21].Value.ToString();
                            }

                            //Col_Time(Finish)
                            if (workSheet.Cells[rowIterator, 22].Value == null)
                            {
                                masterTemplate.Time_Finish = null;
                            }
                            else
                            {
                                masterTemplate.Time_Finish = workSheet.Cells[rowIterator, 22].Value.ToString();
                            }

                            //Col_Result
                            if (workSheet.Cells[rowIterator, 23].Value == null)
                            {
                                masterTemplate.Result = null;
                            }
                            else
                            {
                                masterTemplate.Result = workSheet.Cells[rowIterator, 23].Value.ToString();
                            }

                            //Col_Mc
                            if (workSheet.Cells[rowIterator, 24].Value == null)
                            {
                                masterTemplate.MC = null;
                                patient.MC_File_Name = null;
                            }
                            else
                            {
                                masterTemplate.MC = "MC_" + masterTemplate.LN + "_" + masterTemplate.First_Name + "_" + masterTemplate.Last_Name + ".pdf";
                                patient.MC_File_Name = "MC_" + masterTemplate.LN + "_" + masterTemplate.First_Name + "_" + masterTemplate.Last_Name + ".pdf";
                            }

                            //Col_Email
                            if (workSheet.Cells[rowIterator, 25].Value == null)
                            {
                                masterTemplate.E_mail = null;
                                patient.E_mail = null;
                            }
                            else
                            {
                                masterTemplate.E_mail = workSheet.Cells[rowIterator, 25].Value.ToString();
                                patient.E_mail = workSheet.Cells[rowIterator, 25].Value.ToString();
                            }

                            patient.Time_stamp = DateTime.Now.Date;

                            patient.File_Name = masterTemplate.LN + "_" + masterTemplate.First_Name + " " + masterTemplate.Last_Name + ".pdf";

                            masterList.Add(masterTemplate);
                            patientList.Add(patient);
                        }
                    }
                }
                else
                {
                    TempData["ErrorFile"] = "กรุณาเลือกไฟล์ Excel หรือ ข้อมูลในตารางว่างเปล่า";

                    return RedirectToAction("FileNotSupport", "AdminPanel");
                }

                //LogUpload Header
                logUpload.Upload_Date = DateTime.Now.Date;
                logUpload.Upload_file_name = file.FileName;
                logUpload.Number_of_Records = masterList.Count;
                //---


            }

            using (AccZmedicEntities accZmedicEntities = new AccZmedicEntities())
            {
                foreach (var item in masterList)
                {
                    //ตรวจการซ้ำกันของรหัส LN
                    var LnFromDB = _context.Master_template.FirstOrDefault(mt => mt.LN.Equals(item.LN));

                    if (LnFromDB != null)
                    {
                        TempData["Duplicate"] = "รหัส LN: " + LnFromDB.LN + "___" + LnFromDB.First_Name + "   " + LnFromDB.Last_Name +
                            "   " + LnFromDB.ID_Passport + "___" + "ลำดับที่: " + LnFromDB.Number;

                        //LogUpload failure
                        logUpload.Number_of_Success = 0;
                        logUpload.Number_of_Fails = 1;
                        logUpload.User = Session["Username"].ToString() + "_" + Session["Id"].ToString();
                        logUpload.Upload_Reuslt = "failure";
                        _context.Upload.Add(logUpload);
                        _context.SaveChanges();
                        //---

                        return RedirectToAction("DuplicateLN", "AdminPanel");
                    }
                    else
                    {
                        accZmedicEntities.Master_template.Add(item);
                    }
                }

                foreach (var item in patientList)
                {
                    accZmedicEntities.Patient.Add(item);
                }
                accZmedicEntities.SaveChanges();
            }

            //LogUpload Success
            logUpload.Number_of_Success = masterList.Count;
            logUpload.Number_of_Fails = 0;
            logUpload.User = Session["Username"].ToString() + "_" + Session["Id"].ToString();
            logUpload.Upload_Reuslt = "sucessfully";
            _context.Upload.Add(logUpload);
            _context.SaveChanges();
            //---

            return RedirectToAction("ImportSuccess");

        }

        //------------------------------------- สืบค้นผลแลป -------------------------------------------------------

        public ActionResult PatientLabs()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                var patient = _context.Patient;

                var patientResult = patient.ToList();

                return View(patientResult);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult PatientLabs(string keywords, string dateFrom, string dateTo)
        {
            var patient = _context.Patient;

            var patientResult = patient.ToList();

            if (!string.IsNullOrEmpty(keywords))
            {
                patientResult = patient.Where(p => p.File_Name.Contains(keywords) || p.ID_Passport.Equals(keywords) || p.LN.Contains(keywords)).ToList();

                ViewBag.keywords = keywords;

            }

            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                DateTime dateFromDT = Convert.ToDateTime(dateFrom).AddYears(543);

                DateTime dateToDT = Convert.ToDateTime(dateTo).AddYears(543);

                patientResult = patient.Where(p => p.Collected_Date >= dateFromDT).Where(p => p.Collected_Date <= dateToDT).ToList();

                ViewBag.dateFrom = dateFrom;
                ViewBag.dateTo = dateTo;

            }

            return View(patientResult);
        }

        public ActionResult ExcelExportPatientLabs(string keywords, string dateFrom, string dateTo)
        {

            var FileData = _context.Patient.ToList();

            if (!string.IsNullOrEmpty(keywords))
            {
                FileData = _context.Patient.Where(p => p.File_Name.Contains(keywords) || p.ID_Passport.Equals(keywords) || p.LN.Contains(keywords)).ToList();
            }

            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                DateTime dateFromDT = Convert.ToDateTime(dateFrom).AddYears(543);

                DateTime dateToDT = Convert.ToDateTime(dateTo).AddYears(543);

                FileData = _context.Patient.Where(p => p.Collected_Date >= dateFromDT).Where(p => p.Collected_Date <= dateToDT).ToList();

            }

            try
            {

                DataTable Dt = new DataTable();
                Dt.Columns.Add("ID Passport", typeof(string));
                Dt.Columns.Add("LN", typeof(string));
                Dt.Columns.Add("Collected Date", typeof(string));
                Dt.Columns.Add("Lab Result File Name", typeof(string));
                Dt.Columns.Add("MC File Name", typeof(string));
                Dt.Columns.Add("Date of Birth", typeof(string));
                Dt.Columns.Add("E-mail", typeof(string));
                Dt.Columns.Add("Time Stamp", typeof(string));

                foreach (var data in FileData)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = data.ID_Passport;
                    row[1] = data.LN;
                    row[2] = Convert.ToDateTime(data.Collected_Date).ToString("dd/MM/yyyy");
                    row[3] = data.File_Name;
                    row[4] = data.MC_File_Name;
                    row[5] = data.DOB;
                    row[6] = data.E_mail;
                    row[7] = Convert.ToDateTime(data.Time_stamp).ToString("dd/MM/yyyy");
                    Dt.Rows.Add(row);
                }

                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    worksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(8).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(1).AutoFit();
                    worksheet.Column(2).AutoFit();
                    worksheet.Column(3).AutoFit();
                    worksheet.Column(4).AutoFit();
                    worksheet.Column(5).AutoFit();
                    worksheet.Column(6).AutoFit();
                    worksheet.Column(7).AutoFit();
                    worksheet.Column(8).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult DownloadPatientLabs()
        {

            if (Session["DownloadExcel_FileManager"] != null)
            {
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                var fileName = "PatienLabs" + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                return File(data, "application/octet-stream", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult EditPatientLab(int id)
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                var dataPatientLab = _context.Patient.Where(p => p.Id == id).SingleOrDefault();
                return View(dataPatientLab);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult EditPatientLab(int id, Patient patient)
        {
            var dataPatientLab = _context.Patient.FirstOrDefault(p => p.Id == id);

            if (dataPatientLab != null)
            {
                dataPatientLab.ID_Passport = patient.ID_Passport;
                dataPatientLab.DOB = patient.DOB;

                if(patient.MC_File_Name != null)
                {
                    dataPatientLab.MC_File_Name = "MC_" + dataPatientLab.File_Name;
                }
                else
                {
                    dataPatientLab.MC_File_Name = null;
                }

                dataPatientLab.E_mail = patient.E_mail;

                _context.SaveChanges();
                return RedirectToAction("EditSuccess", "AdminPanel");
            }
            else
            {
                return RedirectToAction("PatientLabs", "AdminPanel");
            }

        }

        //------------------------------------- ประวัติการอัพโหลดไฟล์Excel -------------------------------------------------------

        public ActionResult UploadFilesResult()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                var uploadFiles = _context.Upload;

                var uploadFilesResult = uploadFiles.ToList();

                return View(uploadFilesResult);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public ActionResult UploadFilesResult(string status, string dateFrom, string dateTo)
        {
            var uploadFiles = _context.Upload;

            var uploadFilesResult = uploadFiles.ToList();

            if (!string.IsNullOrEmpty(status))
            {
                uploadFilesResult = uploadFiles.Where(u => u.Upload_Reuslt.Equals(status)).ToList();

                ViewBag.status = status;
            }

            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                DateTime dateFromDT = Convert.ToDateTime(dateFrom).AddYears(543);

                DateTime dateToDT = Convert.ToDateTime(dateTo).AddYears(543);

                uploadFilesResult = uploadFiles.Where(u => u.Upload_Date >= dateFromDT).Where(u => u.Upload_Date <= dateToDT).ToList();

                ViewBag.dateFrom = dateFrom;
                ViewBag.dateTo = dateTo;

            }

            return View(uploadFilesResult);

        }

        public ActionResult ExcelExportUploadFilesResult(string status, string dateFrom, string dateTo)
        {

            var FileData = _context.Upload.ToList();

            if (!string.IsNullOrEmpty(status))
            {
                FileData = _context.Upload.Where(u => u.Upload_Reuslt.Equals(status)).ToList();
            }

            if (!string.IsNullOrEmpty(dateFrom) && !string.IsNullOrEmpty(dateTo))
            {
                DateTime dateFromDT = Convert.ToDateTime(dateFrom).AddYears(543);

                DateTime dateToDT = Convert.ToDateTime(dateTo).AddYears(543);

                FileData = _context.Upload.Where(u => u.Upload_Date >= dateFromDT).Where(u => u.Upload_Date <= dateToDT).ToList();

            }

            try
            {

                DataTable Dt = new DataTable();
                Dt.Columns.Add("Upload Date", typeof(string));
                Dt.Columns.Add("File Name", typeof(string));
                Dt.Columns.Add("Number of Records", typeof(string));
                Dt.Columns.Add("Number of Uploads Success", typeof(string));
                Dt.Columns.Add("Number of Uploads Fails", typeof(string));
                Dt.Columns.Add("Upload By", typeof(string));
                Dt.Columns.Add("Upload Result", typeof(string));

                foreach (var data in FileData)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = Convert.ToDateTime(data.Upload_Date).ToString("dd/MM/yyyy");
                    row[1] = data.Upload_file_name;
                    row[2] = data.Number_of_Records;
                    row[3] = data.Number_of_Success;
                    row[4] = data.Number_of_Fails;
                    row[5] = data.User;
                    row[6] = data.Upload_Reuslt;
                    Dt.Rows.Add(row);
                }

                var memoryStream = new MemoryStream();
                using (var excelPackage = new ExcelPackage(memoryStream))
                {
                    var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                    worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                    worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                    worksheet.DefaultRowHeight = 18;


                    worksheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(3).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(4).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(5).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(6).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.Column(7).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    worksheet.DefaultColWidth = 20;
                    worksheet.Column(1).AutoFit();
                    worksheet.Column(2).AutoFit();
                    worksheet.Column(3).AutoFit();
                    worksheet.Column(4).AutoFit();
                    worksheet.Column(5).AutoFit();
                    worksheet.Column(6).AutoFit();
                    worksheet.Column(7).AutoFit();

                    Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult DownloadUploadFilesResult()
        {

            if (Session["DownloadExcel_FileManager"] != null)
            {
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                var fileName = "UploadFilesResult" + "_" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".xlsx";
                return File(data, "application/octet-stream", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }

        //------------------------------------- สถานะต่าง ๆ -------------------------------------------------------

        public ActionResult ImportSuccess()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult NullAlert()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DuplicateLN()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult FileNotSupport()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult EditSuccess()
        {
            if (Session["Role"] != null && Session["Role"].Equals("1"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

    }
}