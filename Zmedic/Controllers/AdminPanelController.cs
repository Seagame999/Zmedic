using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zmedic.Models;

namespace Zmedic.Controllers
{
    [HandleError]
    public class AdminPanelController : Controller
    {
        AccZmedicEntities _context = new AccZmedicEntities();


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
                                masterTemplate.DOB = Convert.ToDateTime(workSheet.Cells[rowIterator, 6].Value);
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

            return RedirectToAction("ImportSuccess");

        }

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

    }
}