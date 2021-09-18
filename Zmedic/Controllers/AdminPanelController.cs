using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zmedic.Models;

namespace Zmedic.Controllers
{
    public class AdminPanelController : Controller
    {
        AccZmedicEntities _context = new AccZmedicEntities();

        // GET: AdminPanel
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ImportExcelFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportExcelFilePrograms()
        {
            return View();
        }

        public List<Master_template> GetDataFromExcelFile(Stream stream)
        {
            List<Master_template> csvList = new List<Master_template>();

            try
            {
                using (var reader = ExcelReaderFactory.CreateCsvReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    });

                    if (dataSet.Tables.Count > 0)
                    {
                        var dataTable = dataSet.Tables[0];

                        foreach (DataRow objDataRow in dataTable.Rows)
                        {
                            if (objDataRow.ItemArray.All(x => string.IsNullOrEmpty(x?.ToString()))) continue;

                            csvList.Add(new Master_template()

                            {
                                Data_Status = true,
                                Number = Convert.ToInt32(objDataRow["Number"].ToString()),
                                Prefix = objDataRow["Prefix"].ToString(),
                                First_Name = objDataRow["First Name"].ToString(),
                                Last_Name = objDataRow["Lastname"].ToString(),
                                ID_Passport = objDataRow["ID card/Passport ID"].ToString(),
                                DOB = Convert.ToDateTime(objDataRow["DOB"].ToString()),
                                AGE = objDataRow["Age"].ToString(),
                                Sex = objDataRow["Sex"].ToString(),
                                Collected_Date = Convert.ToDateTime(objDataRow["Collected date"].ToString()),
                                Specimen = objDataRow["Specimen"].ToString(),
                                Hospital_Clinic = objDataRow["Hospital/Clinic"].ToString(),
                                Doctor = objDataRow["Doctor"].ToString(),
                                VN = objDataRow["VN"].ToString(),
                                LN = objDataRow["LN"].ToString(),
                                HN = objDataRow["HN"].ToString(),
                                N_gene_Ct = objDataRow["N gene : Ct"].ToString(),
                                S_gene_Ct = objDataRow["S gene : Ct"].ToString(),
                                Date_start = objDataRow["Time (Start)"].ToString(),
                                Time_Finish = objDataRow["Time (Finish)"].ToString(),
                                Result = objDataRow["Result"].ToString(),
                                MC = objDataRow["MC"].ToString()
                            });

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return csvList;
        }


    }
}