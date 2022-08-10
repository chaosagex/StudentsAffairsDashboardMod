using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsAffairsDashboard.Models
{
    public class StudentMainsInvoiceViewModel
    {
        public StudentsMain student { get; set; }
        public decimal debt { get; set; }
        public List<payment_details> paid;
        public List<payment_details> left;
    }
}