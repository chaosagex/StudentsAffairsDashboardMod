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
    
    public partial class payment_details
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public payment_details()
        {
            this.invoice_payment = new HashSet<invoice_payment>();
        }
    
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }
        public decimal amount { get; set; }
        public int school { get; set; }
        public string year { get; set; }
        public short student_type { get; set; }
        public Nullable<int> Grade { get; set; }
    
        public virtual NESSchool NESSchool { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<invoice_payment> invoice_payment { get; set; }
        public virtual Grade Grade1 { get; set; }
    }
}
