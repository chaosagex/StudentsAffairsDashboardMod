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
    using System.Collections.Generic;
    
    public partial class StudentClothe
    {
        public int SCID { get; set; }
        public int StdCode { get; set; }
        public int ClothesID { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string ReceivingStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string ReceivingQuantity { get; set; }
        public string PackageStatus { get; set; }
        public Nullable<int> InvoiceID { get; set; }
    
        public virtual StudentsMain StudentsMain { get; set; }
        public virtual Cloth Cloth { get; set; }
    }
}
