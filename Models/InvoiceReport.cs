using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMCS.Models
{
    public class InvoiceReport
    {
        public string ClaimID { get; set; }
        public string UserName { get; set; }
        public string FacultyName { get; set; }
        public decimal TotalHoursWorked { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
