//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StudentsAffairsDashboard.Models
{
    using System;
    
    public partial class getInvoicesById_Result
    {
        public Nullable<int> id { get; set; }
        public Nullable<int> student { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<decimal> total_cost { get; set; }
        public Nullable<decimal> paid { get; set; }
        public Nullable<decimal> remaining { get; set; }
        public Nullable<int> previous_payment { get; set; }
        public string Notes { get; set; }
    }
}
