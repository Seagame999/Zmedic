using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Zmedic.FileValidation
{
    public class ExcelFileValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                HttpPostedFileWrapper file = (HttpPostedFileWrapper)value;
                string extention = Path.GetExtension(file.FileName);
                string excel1 = ".xls", excel2 = ".XLS", excel3 = ".xlsx", excel4 = ".XLSX";

                if (string.Equals(extention, excel1) == true || string.Equals(extention, excel2) == true || string.Equals(extention, excel3) == true || string.Equals(extention, excel4) == true )
                {
                    return ValidationResult.Success;
                }

                return new ValidationResult("กรุณาใส่ Excel File (.xls , .xlsx)");
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}